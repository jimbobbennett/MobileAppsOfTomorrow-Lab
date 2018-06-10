# Implement an Android-specific version of the AzureService

Due to a quirk with NuGet dependencies, the Mobile Client NuGet package will not work correctly in your Android app due to a dependency issue on some other Android packages. To stop this issue occurring you will need to install the latest version of the "Xamarin.Android.Support.CustomTabs" Android support library package first.

1. Add the "Xamarin.Android.Support.CustomTabs" NuGet package to the `HappyXamDevs.Android` project.

    > Note: Earlier we added a NuGet package to every project in the Solution. This time we only need the Nuget package on the Android project. If you are not sure how to do this, ask your instructor!

2. Add a new folder to the `HappyXamDevs.Android` project called `Services`, and add a new class to that folder called `AzureService`.

3. Make this class public and derive from `AzureServiceBase`. You'll need to add a using statement for the `HappyXamDevs.Services` namespace.

    ```cs
    public class AzureService : AzureServiceBase
    ...
    ```

4. You next need to implement the abstract `AuthenticateUser` method. This method will call the `LoginAsync` method on the mobile service client, and this call needs the current Activity to be passed in. Activities represent full screen views inside your app, and the login method uses the current Activity to launch a web view to allow you to log in. You can easily get the current Activity using the "Plugin.CurrentActivity" NuGet package, so add this to your Android app.

    Once the NuGet package is added, open the `MainActivity` class, and add the following to the `OnCreate` method, just before the call to `LoadApplication`.

    ```cs
    protected override void OnCreate(Bundle bundle)
    {
        ...
        Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, bundle);
        LoadApplication(new App());
    }
    ```

5. Implement the `AuthenticateUser` method in the `AzureService` class, calling the `LoginAsync` method on the mobile service client, requesting a login using Facebook. You will need to add using statements for the `Plugin.CurrentActivity` and `Microsoft.WindowsAzure.MobileServices` namespaces. Note that we also declare this method `async` because of the use of the keyword `await` inside the method.

    ```cs
    protected override async Task AuthenticateUser()
    {
        await Client.LoginAsync(CrossCurrentActivity.Current.Activity,
                                MobileServiceAuthenticationProvider.Facebook,
                                AzureAppName);
    }
    ```

## Register the Android-specific version with the Dependency Service

Xamarin.Forms provides a [dependency service](https://docs.microsoft.com/xamarin/xamarin-forms/app-fundamentals/dependency-service/?WT.mc_id=mobileappsoftomorrow-workshop-jabenn). This is a way to create platform specific implementations and access them via their interface from your cross-platform code. You register a type inside this service in your platform specific code, then request it by interface in your cross-platform code. The first time the interface is requested, a new instance of the platform specific class is created, and this is reused for subsequent calls.

To register the platform-specific `AzureService`, add the following code to the `AzureService` class file, *before* the namespace declaration.

```cs
[assembly: Xamarin.Forms.Dependency(typeof(HappyXamDevs.Droid.Services.AzureService))]
```

The completed `AzureService` class is shown below:

```cs
using System.Threading.Tasks;
using HappyXamDevs.Services;
using Microsoft.WindowsAzure.MobileServices;
using Plugin.CurrentActivity;

[assembly: Xamarin.Forms.Dependency(typeof(HappyXamDevs.Droid.Services.AzureService))]
namespace HappyXamDevs.Android.Services
{
    public class AzureService : AzureServiceBase
    {
        protected override async Task AuthenticateUser()
        {
            await Client.LoginAsync(CrossCurrentActivity.Current.Activity,
                                    MobileServiceAuthenticationProvider.Facebook,
                                    AzureAppName);
        }
    }
}
```

## Configure the Manifest

The call to the login method will launch a webview that redirects to the Facebook login page, and from there you can enter your Facebook credentials. After logging in, you will need to return to your app, and this is something that a web view can't do directly. Instead your app needs to register a _URL scheme_ that it can handle, similar to how mail apps can handle `mailto://<email>` URLs. Apps can register custom URLs with the Android OS, so that when a web view loads these URLs, it is redirected to your app to handle.

When you configured the Facebook authentication in your Function app you already set up a return URL - this is the value you set in the _Allowed external redirect URLs_ field when configuring authentication in the Function app. You now need to configure your Android app to be able to handle this URL. This configuration is done inside the app's `AndroidManifest.xml` file, so find this file inside the `Properties` folder in the Android app and open it.

Inside this XML file, find the `application` node. Add the following code inside this node.

```xml
<activity android:name="com.microsoft.windowsazure.mobileservices.authentication.RedirectUrlActivity" android:launchMode="singleTop" android:noHistory="true">
  <intent-filter>
    <action android:name="android.intent.action.VIEW" />
    <category android:name="android.intent.category.DEFAULT" />
    <category android:name="android.intent.category.BROWSABLE" />
    <data android:scheme="happyxamdevs" android:host="easyauth.callback" />
  </intent-filter>
</activity>
```

> IMPORTANT: Note the `data` node - `<data android:scheme="happyxamdevs" android:host="easyauth.callback" />`. The value of the `android:scheme` attribute must be set to be your Functions App name, for example `happyxamdevs`. This needs to match the first part of the _Allowed external redirect URLs_ you configured in your Azure function app, so if your redirect URL was `happyxamdevs://easyauth.callback` then the _URL scheme_ would be `happyxamdevs` without the `://easyauth.callback`.

## Next step

Now that your Android app authentication has been implemented, the next step is to implement the same for iOS and Windows if you haven't done so already.

* [For iOS](./3_2-CreateAnAzureServiceInTheMobileAppIos.md)
* [For Windows](./3_3-CreateAnAzureServiceInTheMobileAppWin.md)

Once you have set up authentication in all your apps, it is time to [create the Login page](./4-CreateLoginPage.md).