# Create a login page

Now that we have our AzureService configured, it's time to add a login page.

The app will have two pages - the main page showing our Happy Xamarin Developers, and a login page.

![The flow of our mobile app](../Images/AppFlow.png)

We'll be using the [MVVM design pattern](https://docs.microsoft.com/xamarin/xamarin-forms/enterprise-application-patterns/mvvm/?WT.mc_id=mobileappsoftomorrow-workshop-jabenn) for these pages as it is a simple way to get started and is baked in to Xamarin.Forms.

## 1. Create the Login Page

We are going to create a `ContentPage`, a Xamarin.Forms control that contains some form of content, normally a layout control containing other controls.

1. (PC) In the Visual Studio Solution Explorer, right-click **HappyXamDevs** > **Add** > **New Item...**

    - (Mac) In the Visual Studio Solution Explorer, right-click **HappyXamDevs** > **Add** > **New File**

2. (PC) In the **Add New Item** window, select **Installed** > **Visual C# Items** > **Xamarin.Forms** > **Content Page*

    > **Warning:** Select **Content Page**, _not_ **Content Page (C#)**
    - (Mac) In the **New File** window, select **Forms** > **Forms ContentPage XAML**
    > **Wanring:** Select Forms ContentPage XAML, _not_ **Forms ContentPage**

3. (PC) In the **Add New Item** window, set the name to be `LoginPage.xaml`

    - (Mac) In the **New File** set the name to be `LoginPage.xaml`

4. (PC) In the **Add New Item** window, click **Add**

    - (Mac) In the **New File** window, click **Add**

     ![Creating a content page using VS2017](../Images/VS2017CreateLoginPage.png)

     ![Creating a content page using VS2017](../Images/VSMCreateLoginPage.png)

5. In the Visual Studio Solution Explorer, open **HappyXamDevs** > **LoginPage.xaml**

6. In the `LoginPage.xaml` editor, replace the provided template with the following code:

```xml
<?xml version="1.0" encoding="UTF-8"?>
<ContentPage 
    xmlns="http://xamarin.com/schemas/2014/forms" 
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml" 
    xmlns:ios="clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core" 
    ios:Page.UseSafeArea="true" 
    x:Class="HappyXamDevs.LoginPage">
</ContentPage>
```

> **About the Code**

> `ios:Page.UseSafeArea="true"` enables [Safe Areas](https://blog.xamarin.com/making-ios-11-even-easier-xamarin-forms/?WT.mc_id=mobileappsoftomorrow-workshop-jabenn) for iOS; this ensures that the UI will not overlap the iPhone notch

7. In the `LoginPage.xaml` editor, add the `ContentPage.Content`:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:ios="clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core"
             ios:Page.UseSafeArea="true"
             x:Class="HappyXamDevs.LoginPage">

    <ContentPage.Content>
        <Grid>
            <Grid.RowDefinitions>
                <RowDefinition Height="*" />
                <RowDefinition Height="Auto" />
                <RowDefinition Height="*" />
            </Grid.RowDefinitions>

            <Image Grid.Row="0"
                   Source="Bit_Learning.png"
                   Aspect="AspectFit"
                   Margin="50" />

            <Button Margin="20,0"
                    Grid.Row="1"
                    Text="Log in with Facebook"
                    TextColor="White" />
        </Grid>
    </ContentPage.Content>
</ContentPage>  
```

> **About the Code**

> `Grid` is the layout for our ContentPage. `Grid` allows us to place UI controls into specified rows and columns on the page. We have created a `Grid` with 3 rows.

> `Button` is a UI control that the user can tap on. We have placed our Button in. We have added our Button to Row 0.

> `Image` allows us to display an image on the screen. The source for our image is a png file, `Bit_Learning.png` (which we will be importing to our project in the next section). We have added our Image to Row 1.

## 2. Import Bit_Learning.png

The image file must be added to each app project because iOS, Android and UWP handle images slightly differently.

The image is provided in multiple resolutions and, at runtime, each device will automatically select the correct image to display based on its screen density, ensuring the image is not pixelated, regardless of the screen density.

You can read more about this [in the Xamarin docs](https://docs.microsoft.com/xamarin/xamarin-forms/user-interface/images?tabs=vswin&WT.mc_id=mobileappsoftomorrow-workshop-jabenn).

### 2a. Import Bit_Learning.png, Android

Android stores images in the `Resources/drawable` folders, with different `drawable` folders for different device resolutions - so `drawable-hdpi` for high density screens, `drawable-xhdpi` for extra high density screens and so on. Images of different resolutions are put into these folders.

1. In the Visual Studio Solution Explorer, navigate to **HappyXamDevs.Android** > **Resources**

2. In the Visual Studio Solution Explorer, note the many `drawable` folders
    >  **Note:** The lowest-resolution image will be added to `drawable-hdpi` while the highest-resolution image will be added to `drawable-xxxhdpi`

3. (PC) In the Visual Studio Solution Explorer, right-click on the  **drawable-hdpi** folder > **Add** > **Existing Item...**

    - (Mac) In the Visual Studio Solution Explorer, right-click on  **drawable-hdpi** > **Add** > **Add Files...**

4. In the file explorer window, navigate to **MobileAppsOfTomorrow-Lab** > **Assets** > **Android** > **drawable-hdpi**

5. In the file explorer window, double-click **Bit_Learning.png**

6. (PC) _Skip this step_
    - (Mac) In the confirmation popup, select **Copy the file to the directory** > **OK**

7. (PC) In the Visual Studio Solution Explorer, right-click on the  **drawable-xhdpi** folder > **Add** > **Existing Item...**

    - (Mac) In the Visual Studio Solution Explorer, right-click on  **drawable-xhdpi** > **Add** > **Add Files...**

8. In the file explorer window, navigate to **MobileAppsOfTomorrow-Lab** > **Assets** > **Android** > **drawable-xhdpi**

9. In the file explorer window, double-click **Bit_Learning.png**

10. (PC) _Skip this step_
    - (Mac) In the confirmation popup, select **Copy the file to the directory** > **OK**

11. (PC) In the Visual Studio Solution Explorer, right-click on the  **drawable-xxhdpi** folder > **Add** > **Existing Item...**

    - (Mac) In the Visual Studio Solution Explorer, right-click on  **drawable-xxhdpi** > **Add** > **Add Files...**

12. In the file explorer window, navigate to **MobileAppsOfTomorrow-Lab** > **Assets** > **Android** > **drawable-xxhdpi**

13. In the file explorer window, double-click **Bit_Learning.png**

14. (PC) _Skip this step_
    - (Mac) In the confirmation popup, select **Copy the file to the directory** > **OK**

15. (PC) In the Visual Studio Solution Explorer, right-click on the  **drawable-xxxhdpi** folder > **Add** > **Existing Item...**

    - (Mac) In the Visual Studio Solution Explorer, right-click on  **drawable-xxxhdpi** > **Add** > **Add Files...**

16. In the file explorer window, navigate to **MobileAppsOfTomorrow-Lab** > **Assets** > **Android** > **drawable-xxxhdpi**

17. In the file explorer window, double-click **Bit_Learning.png**

18. (PC) _Skip this step_
    - (Mac) In the confirmation popup, select **Copy the file to the directory** > **OK**

### 2b. Import Bit_Learning.png, iOS

iOS uses asset catalogs to manage images. For each image, we create a named image set with 3 different sizes - original size, 2x the resolution and 3x the resolution.

At runtime, the iOS device will select the appropriate image based on its screen resolution.

1. (PC) In the Visual Studio Solution Explorer, open **HappyXamDevs.iOS** >  **Asset Catalogs** > **Assets**
    - (Mac) In the Visual Studio Solution Explorer, open **HappyXamDevs.iOS** >  **Assets.xcassets**

2. (PC) In the **Assets.xcassets** window, select **Add** (box with green **+**) > **Add Image Set**
    - (Mac) In the **Assets.xcassets** window, on the left-hand menu, right-click **AppIcon** > **New Image Set**

3. In the **Assets.xcassets** window, rename the newly created Image set from **Images** to **Bit_Learning**
    > **Note:** The name is case-sensitive and must contain the underscore

4. In the **Bit_Learning** catalog, select the first **1x** box
    > **Note:** (Mac) Be sure to select the top-most **1x** box, directly above **Universal**. We will not be using the rows below, marked **iOS** **iPad**, **Apple Watch** and **Car**.

5. In the file explorer window, navigate to **MobileAppsOfTomorrow-Lab** > **Assets** > **iOS**

6. In the file explorer, double-click **Bit_Learning@1x.png**

7. In the **Bit_Learning** catalog, select the first **2x** box
    > **Note:** (Mac) Be sure to select the top-most **2x** box, directly above **Universal**. We will not be using the rows below, marked **iOS** **iPad**, **Apple Watch** and **Car**.

8. In the file explorer window, navigate to **MobileAppsOfTomorrow-Lab** > **Assets** > **iOS**

9. In the file explorer, double-click **Bit_Learning@3x.png**

10. In the **Bit_Learning** catalog, select the first **3x** box
    > **Note:** (Mac) Be sure to select the top-most **3x** box, directly above **Universal**. We will not be using the rows below, marked **iOS** **iPad**, **Apple Watch** and **Car**.

11. In the file explorer window, navigate to **MobileAppsOfTomorrow-Lab** > **Assets** > **iOS**

12. In the file explorer, double-click **Bit_Learning@3x.png**

13. In Visual Studio, save the **Assesst.xcassets** file by selecting **File** > **Save**

### 2c. Import Bit_Learning.png, UWP

Images are added to the root folder of the UWP project; it is not recommended to add pictures to a sub-folder for a Xamarin.Forms.UWP app.

When the application runs, the UWP framework will automatically select the best looking resolution based on the screen.

> **Note:** If using Visual Studio for Mac, skip this step

1. In the Visual Studio Solution Explorer, right-click **HappyXamDevs.UWP** > **Add** > **Existing Item...**

2. In the file explorer, navigate to **MobileAppsOfTomorrow-Lab** > **Assets** > **UWP**

3. In the file explorer, double-click **Bit_Learning.scale-100**

4. In the Visual Studio Solution Explorer, right-click **HappyXamDevs.UWP** > **Add** > **Existing Item...**

5. In the file explorer, navigate to **MobileAppsOfTomorrow-Lab** > **Assets** > **UWP**

6. In the file explorer, double-click **Bit_Learning.scale-200**

7. In the Visual Studio Solution Explorer, right-click **HappyXamDevs.UWP** > **Add** > **Existing Item...**

8. In the file explorer, navigate to **MobileAppsOfTomorrow-Lab** > **Assets** > **UWP**

9. In the file explorer, double-click **Bit_Learning.scale-300**

## 3. Styling the Login Button

1. In the Visual Studio Solution Explorer, open **HappyXamDevs** > **App.xaml**

2. In the **App.xaml** editor, enter the following code to define a new color, `CoolPurple`:

```xml
<?xml version="1.0" encoding="utf-8"?>
<Application
    xmlns="http://xamarin.com/schemas/2014/forms" 
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml" 
    x:Class="HappyXamDevs.App">
    <Application.Resources>
        <ResourceDictionary>
            <Color x:Key="CoolPurple">#b2169c</Color>
        </ResourceDictionary>
    </Application.Resources>
</Application>
```

> **About the Code**

> `ResourceDictionary` is a static dictionary where we can define run-time constants to be used anywhere in our Xamarin.Forms project

3. In the Visual Studio Solution Explorer, open **HappyXamDevs** > **LoginPage.xaml**

4. In the **LoginPage.xaml** editor, add the `BackgroundColor` using the `StaticResource` we created in the `ResourceDictionary`:

```xml
<Button Margin="20,0"
        Grid.Row="1"
        Text="Log in with Facebook"
        BackgroundColor="{StaticResource CoolPurple}"
        TextColor="White"/>
```

> **About the Code**

> `StaticResource` is a markup extension that tells the code to lookin the `ResourceDictionary` for a resource called `CoolPurple`, and use that value for the property

## 4. Creating the ViewModel

Xamarin.Forms supports data binding, allowing you to wire a page up to a view model and synchronize data back and forth. To make data binding work, the view model must implement the `INotifyPropertyChanged` interface, an interface that contains an event to notify when data changes. The view will detect these changes and update what is displayed. For interactive controls such as buttons, there is a `Command` property on the control that can be bound to a command on the view model - a command being a property of type `ICommand` which is a wrapper for code you can execute.

To provide functionality to the Login page, you will need to create a `LoginViewModel` and bind this to the page.

To help with creating view models, you can create a `BaseViewModel` that provides an implementation of `INotifyPropertyChanged`. Add a folder called `ViewModels` to the core `HappyXamDevs` project, and add a class in that folder called `BaseViewModel`. The code for this class is below.

```cs
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HappyXamDevs.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public bool Set<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(field, value)) return false;

            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
```

This abstract base class implements the `INotifyPropertyChanged` interface which contains a single member - the `PropertyChanged` event. It also has a helper method to allow you to update a field and if the value of the field changes, the `PropertyChanged` event is raised. The `CallerMemberName` attribute on the `propertyName` parameter means you don't have to pass a value to this parameter and the compiler will pass the name of the calling method or property in for you. This is incredibly useful, you can call `Set` from a property setter and the compiler will automatically pass the property name in so that the property change notification is raised for the correct property.

Now you can implement the `LoginViewModel`.

1. Create the `LoginViewModel` class in the `ViewModels` folder, deriving from `BaseViewModel`.

    ```cs
    namespace HappyXamDevs.ViewModels
    {
        public class LoginViewModel : BaseViewModel
        {
        }
    }
    ```

2. The login page has one button that starts the login process, so we need to expose a property of type `ICommand` that implements the login flow. Create a read-only `ICommand` property called `LoginCommand`, adding a using directive for `System.Windows.Input`.

    ```cs
    public ICommand LoginCommand { get; }
    ```

3. Add a constructor to the view model that sets the command using the Xamarin.Forms `Command` class, an implementation of `ICommand` that executes an action when the command is executed, along with the relevant using directives.

    ```cs
    public LoginViewModel()
    {
        LoginCommand = new Command(async () => await Login());
    }
    ```

4. Implement the `Login` method with a call to authenticate the user using the Azure service. If this succeeds, close the current page. You will need to add a using directive for the `HappyXamDevs.Services` namespace.

    ```cs
    private async Task Login()
    {
        var azureService = DependencyService.Get<IAzureService>();
        if (await azureService.Authenticate())
        {
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }
    }
    ```

    > In a production app you wouldn't access the `MainPage` and it's navigation service directly. Instead you would abstract away navigation and use something like the dependency service to resolve it, so that you can unit test your view model.

## Binding the view model

Now you have a view model, it needs to be set as the binding context of the page, and the login button needs to be wired up to the command.

1. Open the `LoginPage.xaml`, and add an XML namespace to the top level `ContentPage` for the `HappyXamDevs.ViewModels` namespace.

    ```xml
    xmlns:viewModels="clr-namespace:HappyXamDevs.ViewModels"
    ```

2. Set the binding context on the top level `ContentPage`, before the `ContentPage.Content` tag.

    ```xml
    <ContentPage.BindingContext>
        <viewModels:LoginViewModel/>
    </ContentPage.BindingContext>
    ```

3. Bind the button to the command on the view model.

    ```xml
    <Button Grid.Row="1"
            Margin="20,0"
            Command="{Binding LoginCommand}"
            Text="Log in with Facebook"
            BackgroundColor="{StaticResource CoolPurple}"
            TextColor="White" />
    ```

## Launch the login page if the user is not logged in

The blank app created when you created the solution just contained a static page. You will need to change this to wrap the page in a `NavigationPage`. This is a page that provides page level navigation - using pages that can navigate back and forth, or by bringing another navigation stack over the top as a modal page. You can read more on navigation pages in the [Xamarin docs](https://docs.microsoft.com/xamarin/xamarin-forms/app-fundamentals/navigation/?WT.mc_id=mobileappsoftomorrow-workshop-jabenn).

1. When your app starts up, the main page is loaded from inside the `App` class. If you open the `App.xaml.cs` file in the `HappyXamDevs` project, you will see the following line in the constructor:

    ```cs
    MainPage = new MainPage();
    ```

    This sets the main page of the application to be a new instance of the `MainPage` class.

2. Change this to wrap the page inside a `NavigationPage` using the code below.

    ```cs
    var navigationPage = new Xamarin.Forms.NavigationPage(new MainPage());
    MainPage = navigationPage;
    ```

3. Add some styling to the navigation page to give a nice purple navigation bar with white text.

    ```cs
    navigationPage.BarBackgroundColor = (Color)Resources["CoolPurple"];
    navigationPage.BarTextColor = Color.White;
    ```

4. Make the navigation page look nice on the iPhoneX by turning on the safe areas. You will need to add using directives for the `Xamarin.Forms.PlatformConfiguration` and `Xamarin.Forms.PlatformConfiguration.iOSSpecific` namespaces.

    ```cs
    navigationPage.On<iOS>().SetPrefersLargeTitles(true);
    navigationPage.On<iOS>().SetUseSafeArea(true);
    ```

5. Inside the `MainPage`, if the app has been launched for the first time check to see if the user is logged in, and if not navigate to the `LoginPage`. Open `MainPage.xaml.cs` and override the `OnAppearing` method with the code below. You will need to add a using directive for the `HappyXamDevs.Services` namespace.

    ```cs
    bool firstAppear = true;

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (!firstAppear) return;
        firstAppear = false;

        var azureService = DependencyService.Get<IAzureService>();

        if (!azureService.IsLoggedIn())
        {
            await Navigation.PushModalAsync(new LoginPage(), false);
        }
    }
    ```

    The `firstAppear` field is used to ensure this check only happens once, when the app is opened. The code then uses the `AzureService` resolved from the dependency service to check to see if the user is logged in. If not, a new `LoginPage` is pushed to the modal navigation stack, covering the main page.

## Test the login flow

Run the app on each platform you are supporting. Click the "Log in with Facebook" button and log in with your Facebook account. Once you authorize access you should be returned to the app and the login page should close.

## Persist the login between sessions

If you run the app more than once, you will see that you have to log in each time. When you log in, you will get an authorization token and a user id from the Azure Function app authentication, and these values can be persisted to the mobile device. That way, the next time the apps is launched, these token and id can be loaded and used to instantiate a user instead of having to log in again.

For the sake of simplicity during this workshop you will be persisting these using un-encrypted Xamarin.Forms [Application properties](https://docs.microsoft.com/dotnet/api/xamarin.forms.application.properties/?WT.mc_id=mobileappsoftomorrow-workshop-jabenn). These exist as a dictionary of strings to objects and is a property on the `Application` instance, available using the `Application.Current` static property.

> __IMPORTANT__ In a production app you should NOT store these values un-encrypted. Instead you should use secure storage, such as the [Secure Storage](https://docs.microsoft.com/xamarin/essentials/secure-storage/?WT.mc_id=mobileappsoftomorrow-workshop-jabenn) capability of the [Xamarin Essentials](https://www.nuget.org/packages/Xamarin.Essentials) NuGet package. You will not be using this here because on iOS this requires a valid provisioning profile, even on the simulator, and setting this up is outside the scope of this workshop.

Open the `AzureServiceBase` class (in the _HappyXamDevs_ project, in the _Services_ folder), as the user will be persisted in this class.

1. Storage is based off key-value pairs, so define two constants as keys to use to store the authorization token and user id.

    ```cs
    const string AuthTokenKey = "auth-token";
    const string UserIdKey = "user-id";
    ```

2. Add a new method that will eventually load the user. This will check to see if there is a current user, and if there is return immediately.

    ```cs
    void TryLoadUserDetails()
    {
        if (Client.CurrentUser != null) return;
    }
    ```

3. Add code to this method to load the user id and authentication token from the application properties. You will need to add a using directive for the `Xamarin.Forms` namespace (note that Intellisense might propose other namespaces for this class, do not get confused!). If the values are present in the application properties, use these to create a user and assign it to the current user property of the mobile service client. These properties are stored as objects, so you will need to call `ToString()` to get them as strings.

    ```cs
    if (Application.Current.Properties.TryGetValue(AuthTokenKey, out var authToken) &&
        Application.Current.Properties.TryGetValue(UserIdKey, out var userId))
    {
        Client.CurrentUser = new MobileServiceUser(userId.ToString())
        {
            MobileServiceAuthenticationToken = authToken.ToString()
        };
    }
    ```

4. The `IsLoggedIn` method needs to call `TryLoadUserDetails` before checking if the user is logged in.

    ```cs
    public bool IsLoggedIn()
    {
        TryLoadUserDetails();
        return Client.CurrentUser != null;
    }
    ```

5. In the `Authenticate` method, after tha successful call to `AuthenticateUser` the authentication token and user id will need to be saved to the application properties. You will also need to call `SavePropertiesAsync` on the current application to save these property values.

    ```cs
    public async Task<bool> Authenticate()
    {
        if (IsLoggedIn()) return true;
        await AuthenticateUser();

        if (Client.CurrentUser != null)
        {
            Application.Current.Properties[AuthTokenKey] = Client.CurrentUser.MobileServiceAuthenticationToken;
            Application.Current.Properties[UserIdKey] = Client.CurrentUser.UserId;
            await Application.Current.SavePropertiesAsync();
        }

        return IsLoggedIn();
    }
    ```

If you now run the app and log in, then restart the app you will no longer need to log in again. Verify this on one platform.

## Next step

Now that your app can authenticate successfully, the next step is to [wire up the camera using a Xamarin plugin](./5-WireUpTheCamera.md).