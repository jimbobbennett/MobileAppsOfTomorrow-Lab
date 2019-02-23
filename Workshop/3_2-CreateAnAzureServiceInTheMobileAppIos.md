# Implement an iOS-specific version of the AzureService

> **Warning:** Complete steps in [3-CreateAnAzureServiceInTheMobileApp](./3-CreateAnAzureServiceInTheMobileApp.md) before beginning the steps below

1. Add a new folder to the `HappyXamDevs.iOS` project called `Services`, and add a new class to that folder called `AzureService`.
2. Make this class derive from `AzureServiceBase`. You'll need to add a using statement for the `HappyXamDevs.Services` namespace.

    ```cs
    public class AzureService : AzureServiceBase
    ...
    ```

3. You next need to implement the abstract `AuthenticateUser` method. This method will call the `LoginAsync` method on the mobile service client, and this call needs a view controller to be passed in. View controllers represent full screen views inside your app (or parts of a screen, for example the tabs on a tab page), and the login method uses the current view controller to launch a web view to allow you to log in. Before you can implement this method you will need to get the top-most view controller to pass to the login call using the following method which you should add to the `AzureService` class. You might also need to add a using directive for the `UIKit` namespace.

     ```cs
    static UIViewController GetTopmostViewController()
    {
        var window = UIApplication.SharedApplication.KeyWindow;
        var vc = window.RootViewController;
        while (vc.PresentedViewController != null)
        {
            vc = vc.PresentedViewController;
        }
        return vc;
    }
    ```

4. Implement the `AuthenticateUser`, calling the `LoginAsync` method on the mobile service client, requesting a login using Facebook. Note that we also declare this method `async` because of the use of the keyword `await` inside the method. You will need to add a using statement for the `Microsoft.WindowsAzure.MobileServices` namespace.

    ```cs
    protected override async Task AuthenticateUser()
    {
        await Client.LoginAsync(GetTopmostViewController(),
                                MobileServiceAuthenticationProvider.Facebook,
                                "happyxamdevs");
    }
    ```

5. Register this class with the dependency service by adding the following code *before* the namespace declaration:

    ```cs
    [assembly: Xamarin.Forms.Dependency(typeof(HappyXamDevs.iOS.Services.AzureService))]
    ```

The completed class is shown below:

```cs
using System.Threading.Tasks;
using HappyXamDevs.Services;
using Microsoft.WindowsAzure.MobileServices;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(HappyXamDevs.iOS.Services.AzureService))]

namespace HappyXamDevs.iOS.Services
{
    public class AzureService : AzureServiceBase
    {
        protected override async Task AuthenticateUser()
        {
            await Client.LoginAsync(GetTopmostViewController(),
                                    MobileServiceAuthenticationProvider.Facebook,
                                    "happyxamdevs");
        }

        static UIViewController GetTopmostViewController()
        {
            var window = UIApplication.SharedApplication.KeyWindow;
            var vc = window.RootViewController;
            while (vc.PresentedViewController != null)
            {
                vc = vc.PresentedViewController;
            }

            return vc;
        }
    }
}
```

## Configure the PList

Just like with Android, you will need to configure the callback URL scheme, and this is done in the `info.plist` file. Open this file from the solution explorer.

1. Head to the _Application_ tab and copy the value of the _Bundle Identifier_. This will be something like `com.companyname.happyxamdevs`.

2. Switch to the _Advanced_ tab, expand the _URL types_ node and click _Add URL type_. Set the _Identifier_ to be the value of your _Bundle Identifier_, and the _URL Schemes_ to be `happyxamdevs`. This matches the first part of the _Allowed external redirect URLs_ you configured in your Azure functions app, so the redirect URL of `happyxamdevs://easyauth.callback` would have a _URL scheme_ of `happyxamdevs` without the `://easyauth.callback`.

   ![Setting the URL scheme](../Images/VS2017AddUriScheme.png)

3. After this is configured, you will need to tell your iOS app what to do when it is called using the URL scheme. Open the `AppDelegate` class and add the following method along with appropriate using directives for the `Xamarin.Forms`, `HappyXamDevs.Services`, `HappyXamDevs.iOS.Services` and `Microsoft.WindowsAzure.MobileServices` namespaces.

    ```cs
    public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
    {
        var azureService = DependencyService.Get<IAzureService>() as AzureService;
        return azureService.Client.ResumeWithURL(url);
    }
    ```

## Next step

Now that your iOS app authentication has been implemented, the next step is to implement the same for Android and Windows if you haven't done so already.

* [For Android](./3_1-CreateAnAzureServiceInTheMobileAppDroid.md)
* [For Windows](./3_3-CreateAnAzureServiceInTheMobileAppWin.md)

Once you have set up authentication in all your apps, it is time to [create the Login page](./4-CreateLoginPage.md).
