# Implement an Android-specific version of the AzureService

> **Warning:** Complete steps in [3-CreateAnAzureServiceInTheMobileApp](./3-CreateAnAzureServiceInTheMobileApp.md) before beginning the steps below

## 1. Adding a NuGet Package, Plugin.CurrentActivity

1. (PC) In Visual Studio, right-click the `HappyXamDevs.Android` project > **Manage NuGet Packages For Solution..**

    - (Mac) In Visual Studio for Mac, right-click the `HappyXamDevs.Android` project > **Add** > **Add NuGet Packages**

2. (PC) In the **NuGet Package Manager** window, select **Browse**

    - (Mac) _Skip this step_

3. In the **NuGet Package Manager** window, in the search bar, enter **Plugin.CurrentActivity**

4. In the **NuGet Package Manager** window, in the search results, select **Plugin.CurrentActivity**

5. (PC) In the **NuGet Package Manager** window, select **Install**

    - (Mac) In the **NuGet Package Manager** window, select **Add Package**

6. In the Visual Studio Solution Explorer, open `HappyXamDevs.Android` > `MainActivity.cs` 

7. In the `MainActivity.cs` editor, enter the following code: 

```csharp
using Android.App;
using Android.Content.PM;
using Android.OS;

namespace HappyXamDevs.Droid
{
    [Activity(Label = "HappyXamDevs", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, bundle);
            LoadApplication(new App());
        }
    }
}
```

> **About the Code**
>
> `Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, bundle);` initialized the `Plugin.CurrentActivity` library

## 2. Create AzureService.cs

1. In the Visual Studio Solution Explorer, right-click on the project `HappyXamDevs.Android` > **Add** > **New Folder**

2. In the Visual Studio Solution Explorer, name the new folder `Services`

3. In the Visual Studio Solution Explorer, right-click on the newly created `Services` folder > **Add** > **Class**

    - (Mac) On Visual Studio for Mac, right-click on the newly created `Services` folder > **Add** > **New File**

4. In the Add New Item window, name the new file `AzureService.cs`

5. In the `AzureService.cs` editor, enter the following code:

```csharp
using System.Threading.Tasks;
using HappyXamDevs.Services;
using Microsoft.WindowsAzure.MobileServices;
using Plugin.CurrentActivity;

[assembly: Xamarin.Forms.Dependency(typeof(HappyXamDevs.Droid.Services.AzureService))]
namespace HappyXamDevs.Droid.Services
{
    public class AzureService : AzureServiceBase
    {
        protected override Task AuthenticateUser()
        {
            return Client.LoginAsync(CrossCurrentActivity.Current.Activity,
                                    MobileServiceAuthenticationProvider.Facebook,
                                    "happyxamdevs");
        }
    }
}
```

> **About the Code**
>
> `Task AuthenticateUser()` calls `LoginAsync` requesting a login using Facebook. The `"happyxamdevs"` parameter is the name of the URL scheme that we registered in the Facebook App's **ALLOWED EXTERNAL REDIRECT URLS** settings
>
> `[assembly: Xamarin.Forms.Dependency(typeof(HappyXamDevs.Droid.Services.AzureService))]` is the [Xamarin.Forms dependency service](https://docs.microsoft.com/xamarin/xamarin-forms/app-fundamentals/dependency-service/?WT.mc_id=mobileappsoftomorrow-workshop-jabenn). This is a way to create a platform-specific implementation and access them from the Xamarin.Forms project. This registers a Android implementation for `IAzureService`.

## 3. Configure the Android Manifest

`AuthenticateUser()` will launch a WebView that redirects to the Facebook login page where we enter our Facebook credentials. To automatically return to the app after logging in, the app needs to register a **URL scheme** that it can handle (similar to how mail apps can handle `mailto://<email>` URLs). Apps can register custom URLs with the Android OS, so that when a web view loads these URLs, it is redirected to your app to handle.

We configured **Allowed external redirect URLs** in the Azure Function App as the return URL. We now need to configure our Android app to be able to handle this URL. This configuration is done in `HappyXamDevs.Android` > `Properties` > `AndroidManifest.xml`.

1. In the Visual Studio Solution Explorer, open `HappyXamDevs.Android` > `Properties` > `AndroidManifest.xml`

2. In the `AndroidManifest.xml` editor, at the botton, select the `Source` tab

3. In the `Source` tab of the `AndroidManifest.xml` editor, enter the following code:

```xml
<?xml version="1.0" encoding="utf-8"?>
<manifest xmlns:android="http://schemas.android.com/apk/res/android" android:versionCode="1" android:versionName="1.0" package="com.companyname.HappyXamDevs" android:installLocation="auto">
	<uses-sdk android:minSdkVersion="21" android:targetSdkVersion="28" />
	<uses-permission android:name="android.permission.INTERNET" />
	<application android:label="HappyXamDevs.Android">
		<activity android:name="com.microsoft.windowsazure.mobileservices.authentication.RedirectUrlActivity" android:launchMode="singleTop" android:noHistory="true">
			<intent-filter>
				<action android:name="android.intent.action.VIEW" />
				<category android:name="android.intent.category.DEFAULT" />
				<category android:name="android.intent.category.BROWSABLE" />
				<data android:scheme="happyxamdevs" android:host="easyauth.callback" />
			</intent-filter>
		</activity>
	</application>
</manifest>
```

> **About the Code**
>
> `<data android:scheme="happyxamdevs" android:host="easyauth.callback" />`  registers your app with the OS as responding to any calls from a website to `happyxamdevs://easyauth.callback`.

## Next step

Now that your Android app authentication has been implemented, the next step is to implement the same for iOS and Windows if you haven't done so already.

* [For iOS](./3_2-CreateAnAzureServiceInTheMobileAppIos.md)
* [For Windows](./3_3-CreateAnAzureServiceInTheMobileAppWin.md)

Once you have set up authentication in all your apps, it is time to [create the Login page](./4-CreateLoginPage.md).