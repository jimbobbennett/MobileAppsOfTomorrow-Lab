# Implement a UWP-specific version of the AzureService

1. Add a new folder to the `HappyXamDevs.UWP` project called `Services`, and add a new class to that folder called `AzureService`.

2. Make this class derive from `AzureServiceBase`. You'll need to add a using statement for the `HappyXamDevs.Services` namespace.

    ```cs
    public class AzureService : AzureServiceBase
    ...
    ```

3. You next need to implement the abstract `AuthenticateUser` method. Unlike Android and iOS this doesn't need access to anything else.

    ```cs
    protected override async Task AuthenticateUser()
    {
        await Client.LoginAsync(MobileServiceAuthenticationProvider.Facebook,
                                AzureAppName);
    }
    ```

The completed class is shown below.

```cs
using System.Threading.Tasks;
using HappyXamDevs.Services;
using Microsoft.WindowsAzure.MobileServices;

namespace HappyXamDevs.UWP.Services
{
    public class AzureService : AzureServiceBase
    {
        protected override async Task AuthenticateUser()
        {
            await Client.LoginAsync(MobileServiceAuthenticationProvider.Facebook,
                                    AzureAppName);
        }
    }
}
```

## Register the Windows-specific version with the Dependency Service

This class needs to be registered with the dependency service, but unlike iOS and Android this can't always be done with an attribute. If your app uses .NET native compilation (and by default, release builds of UWP apps do), then you need to register your service in code. Open the `App.xaml.cs` file from the `HappyXamDevs.UWP` project (**not** the one in the cross-platform `HappyXamDevs` project) by expanding the `App.xaml` node in the Solution explorer, and add the following code in the `OnLaunched` method after the call to `Xamarin.Forms.Forms.Init(e);` :

```cs
Xamarin.Forms.DependencyService.Register<Services.AzureService>();
```

## Configure the URL scheme

Just like with Android and iOS you will need to configure your app to handle the call back from the URL scheme.

1. In the `App.xaml.cs` file, override the `OnActivated` method.

    ```cs
    protected override void OnActivated(IActivatedEventArgs args)
    {
        if (args.Kind == ActivationKind.Protocol)
        {
            var protocolArgs = args as ProtocolActivatedEventArgs;
            var content = Window.Current.Content as Frame;
            if (content.Content.GetType() == typeof(MainPage))
            {
                content.Navigate(typeof(MainPage), protocolArgs.Uri);
            }
        }
        Window.Current.Activate();
        base.OnActivated(args);
    }
    ```

2. Open the `MainPage.xaml.cs` file from the `HappyXamDevs.UWP` app (**not** the one in the cross-platform `HappyXamDevs` project) by expanding the `MainPage.xaml` node in the solution explorer and double-clicking on the .cs file.

3. Override the `OnNavigatedTo` method with code to handle the URL using the mobile service client. You will need to add using directives for the `HappyXamDevs.Services`, `HappyXamDevs.Services.UWP` and `Microsoft.WindowsAzure.MobileServices` namespaces.

    ```cs
    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        if (e.Parameter is Uri)
        {
            var azureService = Xamarin.Forms.DependencyService.Get<IAzureService>() as AzureService;
            azureService.Client.ResumeWithURL(e.Parameter as Uri);
        }
    }
    ```

4. Open the `Package.appxmanifest` file and switch to the _Declarations_ tab. Drop down the _Available Declarations_ list, select _Protocol_ and click "Add". Set the _Display Name_ to be the name of your app (for example "Happy Xamarin Developers"), and the _Name_ to be your Azure Functions app name, for example `happyxamdevs`. This needs to match the first part of the _Allowed external redirect URLs_ you configured in your Azure functions app, so if your redirect URL was `happyxamdevs://easyauth.callback` then the _URL scheme_ would be `happyxamdevs` without the `://easyauth.callback`.

    ![Configuring the UWP protocol](../Images/VS2017ConfigureUWPProtocol.PNG)

## Next step

Now that your UWP app authentication has been implemented, the next step is to implement the same for Android and iOS if you haven't done so already.

* [For Android](./3_1-CreateAnAzureServiceInTheMobileAppDroid.md)
* [For iOS](./3_2-CreateAnAzureServiceInTheMobileAppIos.md)

Once you have set up authentication in all your apps, it is time to [create the Login page](./4-CreateLoginPage.md).
