# Create a login page

Now that you have an Azure service configured, it's time to add a login page. The app will have two pages - a main page that is the app proper, and a login page. The flow of the app is:

![The flow of our mobile app](../Images/AppFlow.png)

We'll be using the [MVVM design pattern](https://docs.microsoft.com/xamarin/xamarin-forms/enterprise-application-patterns/mvvm/?WT.mc_id=mobileappsoftomorrow-workshop-jabenn) for these pages as it is a simple way to get started and is baked in to Xamarin.Forms.

## Creating the page

The page we are going to create is a `ContentPage`, a Xamarin.Forms page control that contains some form of content, normally a layout control containing other controls.

1. Create a new XAML content page in the `HappyXamDevs` core project called `LoginPage`.
   * For Visual Studio 2017 on Windows, right-click on the `HappyXamDevs` project and select _Add->New Item..._. Choose _Visual C#->Xamarin.Forms_ from the tree on the left, then select _Content Page_ (**NOT** _Content Page C#_). Set the name to be `LoginPage` and click "Add".

     ![Creating a content page using VS2017](../Images/VS2017CreateLoginPage.png)

   * For Visual Studio for Mac, right-click on the `HappyXamDevs` project and select _Add->New File..._. Select _Forms_ on the left, then select _Forms ContentPage XAML_, set the name to be `LoginPage` and click "New".

     ![Creating a content page using VS2017](../Images/VSMCreateLoginPage.png)

2. All pages need to work well on iPhone X devices, so add the following attributes to the `ContentPage` tag in the `LoginPage.xaml` file to [turn on the safe areas](https://blog.xamarin.com/making-ios-11-even-easier-xamarin-forms/?WT.mc_id=mobileappsoftomorrow-workshop-jabenn).

    ```xml
    xmlns:ios="clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core"
    ios:Page.UseSafeArea="true"
    ```

    After the change, this is how the opening tag should look like:

    ```xml
    <ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
                 xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
                 xmlns:ios="clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core"
                 ios:Page.UseSafeArea="true"
                 x:Class="HappyXamDevs.LoginPage">
    ```

3. Delete the `StackLayout` from the content page and add the following `Grid` and `Button`. This code creates a `Grid` that fills the page, with a `Button` inside it. Grid are a type of panel that that can be used to layout controls and other panels inside them using a grid of rows and columns. Buttons are button controls that users can tap to trigger an event.

    ```xml
    <Grid>
        <Button Margin="20,0"
                Text="Log in with Facebook"/>
    </Grid>
    ```

    > The `Margin` property sets space around the control. Margins can have a single value, 2 values or 4. A single value puts an equal margin on all 4 sides. If you use 2 values, the first will be the margin on the left and right side, the second is the margin top and bottom. For 4 values, they are the margins for left, top, right then bottom. In this case the margin of "20,0" will put a margin of 20 points on the left and right-hand sides, and no margin top or bottom.

The login page is a bit boring with a simple button on the screen, so you can make it look nicer by adding some colors and a cool image.

## Styling the login button

1. Inside the `Application.Resources` node in the `App.xaml` XAML file, add a new resource dictionary defining a new color called "CoolPurple".

    ```xml
    <ResourceDictionary>
        <Color x:Key="CoolPurple">#b2169c</Color>
    </ResourceDictionary>
    ```

    This defines a named color resource that can be used elsewhere in the app.

2. Use this color as the background for the button on the login page, and set the text to be white so it is more visible against the purple. Inside `LoginPage.xaml` add the `BackgroundColor` and `TextColor` attributes to the button using the following code.

    ```xml
     <Button Margin="20,0"
             Text="Log in with Facebook"
             BackgroundColor="{StaticResource CoolPurple}"
             TextColor="White"/>
    ```

    The `StaticResource` is a markup extension that tells the code to find a resource called `CoolPurple` and use that value for the property.

## Adding an image to the login page

Before we can put an image on the login page, we need to add the image file to the app projects. iOS, Android and UWP all handle images differently, but the general principle is the same. Each image is provided in multiple resolutions as devices can have different screen densities. The aim being that an image will look the same physical size on all devices regardless of the screen density. You can read more about this [in the Xamarin docs](https://docs.microsoft.com/xamarin/xamarin-forms/user-interface/images?tabs=vswin&WT.mc_id=mobileappsoftomorrow-workshop-jabenn).

### Adding the image on Android

Android stores images in the `Resources/drawable` folders, with different `drawable` folders for different device resolutions - so `drawable-hdpi` for high density screens, `drawable-xhdpi` for extra high density screens and so on. Images of different resolutions are put into these folders.

You can find the images already provided in different sizes in the [__Assets__](./Assets/) folder in this GitHub repo. Open the `Android` folder, and add the `Bit_Learning.png` image from the various `drawable` folders into the matching `drawable` folders in the `HappyXamDevs.Android` app. Do this by right-clicking on the `drawable-hdpi` folder and selecting _Add->Existing Item..._ (Visual Studio 2017 on Windows) or _Add->Add Files..._ (Visual Studio for Mac), then navigate to the `Assets/Android/drawable-hdpi` folder in this repo and select the `Bit_Learning.png` file. Repeat this for all the drawable folders in the `Assets` folder.

### Adding the image on iOS

iOS uses asset catalogs to manage images. In these you can create a named image set with 3 different sizes - original size, 2x the resolution and 3x the resolution.

* On Visual Studio 2017 on Windows, you can open the asset catalog by expanding the _Asset Catalogs_ node in the iOS app in the solution explorer and double clicking on the _Assets_ item inside. Click the _Add_ button on the top left (it is a box with a green plus) and select _Add Image Set_. Double click on the _Images_ item that has been created and rename it to "Bit_Learning"

  Once the image set has been created, click the ellipses (...) on the top of the _1X_ box and select `Bit_Learning@1x.png` from the `Assets/iOS` folder. Repeat this for the _2X_ and _3X_ images using the relevant image.

* For Visual Studio for Mac, double-click on _Assets.xcassets_ node in the iOS app, right click on the list of assets on the left and select _New Image Set_.  Double click on the _Image_ item that has been created and rename it to "Bit_Learning"

  Once the image set has been created, click the _1X_ box and select `Bit_Learning@1x.png` from the `Assets/iOS` folder. Repeat this for the _2X_ and _3X_ images using the relevant image.

### Adding the image on UWP

UWP projects only run on Windows so you will use Visual Studio 2017 to add the image.

For UWP, images should be added straight to the root of the UWP project. Unfortunately it is not possible to add pictures to a sub-folder in Xamarin.Forms UWP.

* Right click on the project _HappyXamDevs.UWP_ and select _Add -> Existing Item..._ from the context menu.
* Navigate to the `Assets/UWP` folder in this repo.
* Add the images `Bit_Learning.scale-100.png`, `Bit_Learning.scale-200.png` and `Bit_Learning.scale-300.png` and click on "Add".

When the application runs, the UWP framework will automatically select the best looking resolution based on the screen.

### Adding the image to the Login Page

Once the images have been added to the project, you can add it to the `LoginPage`.

1. Add some rows to the `Grid` on the page. The button should be in the center, with two equally sized rows, one above and one below. To do this, add the following `RowDefinitions` to the `Grid`, above the `Button` tag.

    ```xml
     <Grid.RowDefinitions>
        <RowDefinition Height="*"/>
        <RowDefinition Height="Auto"/>
        <RowDefinition Height="*"/>
    </Grid.RowDefinitions>
    ```

    This creates 3 rows. The center row is sized to fit its content (Auto), so will be the same height as the button. The other two rows are set to take up the remaining space and both be the same size. You can read more on these row sizings in the [Grid documentation](https://docs.microsoft.com/xamarin/xamarin-forms/user-interface/layouts/grid/?WT.mc_id=mobileappsoftomorrow-workshop-jabenn).

2. Move the button to the second row. Rows are numbered from 0, so and a `Grid.Row="1"` attribute to the button to tell the grid to layout the button on the second row.

    ```xml
    <Button Grid.Row="1"
            Margin="20,0"
            Text="Log in with Facebook"
            BackgroundColor="{StaticResource CoolPurple}"
            TextColor="White" />
    ```

3. Add an `Image` inside the `Grid` above the `Button`. Set the source to be `Bit_Learning.png` and the aspect to be `AspectFit`. In the code below the `Grid.Row` is set to 0, but you can leave this out as the default is row 0.

    > The order of elements declared inside the `Grid` has no affect on their positioning in different rows. The element on row 0 can come before or after the element on row 1 and they wil still end up in the right place. The only time the order does matter is if elements are in the same row, then the Z order is controlled by the element position inside the `Grid`, where elements are stacked up in the order in which they are declared.

    ```xml
    <Image Grid.Row="0"
           Source="Bit_Learning.png"
           Aspect="AspectFit"
           Margin="50"/>
    ```

## Creating the view model

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