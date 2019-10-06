# Create a login page

Now that we have our Azure Service configured, it's time to add a login page.

The app will have two pages - the main page showing our Happy Xamarin Developers, and a login page.

![The flow of our mobile app](../Images/AppFlow.png)

We'll be using the [MVVM design pattern](https://docs.microsoft.com/xamarin/xamarin-forms/enterprise-application-patterns/mvvm/?WT.mc_id=mobileappsoftomorrow-workshop-jabenn) for these pages as it is a simple way to get started and is baked in to Xamarin.Forms.

## 1. Creating the Login Page

We are going to create a `Xamarin.Forms.ContentPage`, a Xamarin.Forms control that contains some form of content, normally a layout control containing other controls.

1. (PC) In the **Visual Studio Solution Explorer**, right-click on the second-from-the-top **HappyXamDevs** option > **Add** > **New Item...**

    - (Mac) In the **Visual Studio Solution Explorer**, right-click on the second-from-the-top **HappyXamDevs** option > **Add** > **New File**

    > **Warning:** Do not right-click on top-most **HappyXamDevs** option.

2. (PC) In the **Add New Item** window, select **Installed** > **Visual C# Items** > **Xamarin.Forms** > **Content Page**

    > **Warning:** Select **Content Page**, _not_ **Content Page (C#)**
    - (Mac) In the **New File** window, select **Forms** > **Forms ContentPage XAML**
    > **Wanring:** Select Forms ContentPage XAML, _not_ **Forms ContentPage**

3. (PC) In the **Add New Item** window, set the name to be `LoginPage.xaml`

    - (Mac) In the **New File** set the name to be `LoginPage.xaml`

4. (PC) In the **Add New Item** window, click **Add**

    - (Mac) In the **New File** window, click **Add**

     ![Creating a content page using VS2017](../Images/VS2017CreateLoginPage.png)

     ![Creating a content page using VS2017](../Images/VSMCreateLoginPage.png)

5. In the **Visual Studio Solution Explorer**, open **HappyXamDevs** > **LoginPage.xaml**

6. In the `LoginPage.xaml` editor, replace the provided template with the following code:

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
>
> `ios:Page.UseSafeArea="true"` enables [Safe Areas](https://blog.xamarin.com/making-ios-11-even-easier-xamarin-forms/?WT.mc_id=mobileappsoftomorrow-workshop-jabenn) for iOS; this ensures that the UI will not overlap the iPhone notch
>
> `Grid` is the layout for our ContentPage. `Grid` allows us to place UI controls into specified rows and columns on the page. We have created a `Grid` with 3 rows.
>
> `Button` is a UI control that the user can tap on. We have placed our Button in. We have added our Button to Row 0.
>
> `Image` allows us to display an image on the screen. The source for our image is a png file, `Bit_Learning.png` (which we will be importing to our project in the next section). We have added our Image to Row 1.

## 2. Importing Bit_Learning.png for our Image

The png file must be added to each app project because iOS, Android and UWP handle images slightly differently.

The image is provided in multiple resolutions and, at runtime, each device will automatically select the correct image to display based on its screens pixel-density, ensuring the image is not pixelated, regardless of the screen size.

You can read more about this [in the Xamarin docs](https://docs.microsoft.com/xamarin/xamarin-forms/user-interface/images?tabs=vswin&WT.mc_id=mobileappsoftomorrow-workshop-jabenn).

### 2a. Importing Bit_Learning.png, Android

Android stores images in the `Resources/drawable` folders, with different `drawable` folders for different device resolutions; `drawable-hdpi` for high density screens, `drawable-xhdpi` for extra high density screens, etc. Images of different resolutions are put into these folders.

1. In the **Visual Studio Solution Explorer**, navigate to **HappyXamDevs.Android** > **Resources**

2. In the **Visual Studio Solution Explorer**, note the many `drawable` folders
    >  **Note:** The lowest-resolution image will be added to `drawable-hdpi` while the highest-resolution image will be added to `drawable-xxxhdpi`

3. (PC) In the **Visual Studio Solution Explorer**, right-click on the  **drawable-hdpi** folder > **Add** > **Existing Item...**

    - (Mac) In the **Visual Studio Solution Explorer**, right-click on  **drawable-hdpi** > **Add** > **Add Files...**

4. In the file explorer window, navigate to **MobileAppsOfTomorrow-Lab** > **Assets** > **Android** > **drawable-hdpi**

5. In the file explorer window, double-click **Bit_Learning.png**

6. (PC) _Skip this step_
    - (Mac) In the confirmation popup, select **Copy the file to the directory** > **OK**

7. (PC) In the **Visual Studio Solution Explorer**, right-click on the  **drawable-xhdpi** folder > **Add** > **Existing Item...**

    - (Mac) In the **Visual Studio Solution Explorer**, right-click on  **drawable-xhdpi** > **Add** > **Add Files...**

8. In the file explorer window, navigate to **MobileAppsOfTomorrow-Lab** > **Assets** > **Android** > **drawable-xhdpi**

9. In the file explorer window, double-click **Bit_Learning.png**

10. (PC) _Skip this step_
    - (Mac) In the confirmation popup, select **Copy the file to the directory** > **OK**

11. (PC) In the **Visual Studio Solution Explorer**, right-click on the  **drawable-xxhdpi** folder > **Add** > **Existing Item...**

    - (Mac) In the **Visual Studio Solution Explorer**, right-click on  **drawable-xxhdpi** > **Add** > **Add Files...**

12. In the file explorer window, navigate to **MobileAppsOfTomorrow-Lab** > **Assets** > **Android** > **drawable-xxhdpi**

13. In the file explorer window, double-click **Bit_Learning.png**

14. (PC) _Skip this step_
    - (Mac) In the confirmation popup, select **Copy the file to the directory** > **OK**

15. (PC) In the **Visual Studio Solution Explorer**, right-click on the  **drawable-xxxhdpi** folder > **Add** > **Existing Item...**

    - (Mac) In the **Visual Studio Solution Explorer**, right-click on  **drawable-xxxhdpi** > **Add** > **Add Files...**

16. In the file explorer window, navigate to **MobileAppsOfTomorrow-Lab** > **Assets** > **Android** > **drawable-xxxhdpi**

17. In the file explorer window, double-click **Bit_Learning.png**

18. (PC) _Skip this step_
    - (Mac) In the confirmation popup, select **Copy the file to the directory** > **OK**

### 2b. Import Bit_Learning.png, iOS

iOS uses asset catalogs to manage images. For each image, we create a named image set with 3 different sizes - original size, 2x the resolution and 3x the resolution.

At runtime, the iOS device will select the appropriate image based on its screen resolution.

1. (PC) In the **Visual Studio Solution Explorer**, open **HappyXamDevs.iOS** >  **Asset Catalogs** > **Assets**
    - (Mac) In the **Visual Studio Solution Explorer**, open **HappyXamDevs.iOS** >  **Assets.xcassets**

2. (PC) In the **Assets.xcassets** window, select **Add** (box with green **+**) > **Add Image Set**
    - (Mac) In the **Assets.xcassets** window, on the left-hand menu, right-click **AppIcon** > **New Image Set**

3. In the **Assets.xcassets** window, rename the newly created Image set from **Image** to **Bit_Learning**
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

13. In Visual Studio, save the **Assesst.xcassets** file by selecting **File** > **Save All**

### 2c. Importing Bit_Learning.png, UWP

Images are added to the root folder of the UWP project; it is not recommended to add pictures to a sub-folder for a Xamarin.Forms.UWP app.

When the application runs, the UWP framework will automatically select the best looking resolution based on the screen.

> **Note:** If using Visual Studio for Mac, skip this step

1. In the **Visual Studio Solution Explorer**, right-click **HappyXamDevs.UWP** > **Add** > **Existing Item...**

2. In the file explorer, navigate to **MobileAppsOfTomorrow-Lab** > **Assets** > **UWP**

3. In the file explorer, double-click **Bit_Learning.scale-100**

4. In the **Visual Studio Solution Explorer**, right-click **HappyXamDevs.UWP** > **Add** > **Existing Item...**

5. In the file explorer, navigate to **MobileAppsOfTomorrow-Lab** > **Assets** > **UWP**

6. In the file explorer, double-click **Bit_Learning.scale-200**

7. In the **Visual Studio Solution Explorer**, right-click **HappyXamDevs.UWP** > **Add** > **Existing Item...**

8. In the file explorer, navigate to **MobileAppsOfTomorrow-Lab** > **Assets** > **UWP**

9. In the file explorer, double-click **Bit_Learning.scale-300**

## 3. Styling the Login Button

1. In the **Visual Studio Solution Explorer**, open **HappyXamDevs** > **App.xaml**

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
>
> `ResourceDictionary` is a static dictionary where we can define run-time constants to be used anywhere in our Xamarin.Forms project

3. In the **Visual Studio Solution Explorer**, open **HappyXamDevs** > **LoginPage.xaml**

4. In the **LoginPage.xaml** editor, add the `BackgroundColor` using the `StaticResource` we created in the `ResourceDictionary`:

```xml
<Button Margin="20,0"
        Grid.Row="1"
        Text="Log in with Facebook"
        BackgroundColor="{StaticResource CoolPurple}"
        TextColor="White"/>
```

> **About the Code**
>
> `StaticResource` is a markup extension that tells the code to lookin the `ResourceDictionary` for a resource called `CoolPurple`, and use that value for the property

## 4. Creating the ViewModel

Xamarin.Forms supports data binding, allowing us to link a page to a ViewModel and synchronize data back and forth. To make data binding work, the ViewModel must implement `INotifyPropertyChanged`, an interface that contains an event to notify when data changes. The view will detect these changes and update what is displayed. 

For interactive controls such as buttons, there is a `Command` property on the control that can be bound to a command on the view model - a command being a property of type `ICommand` which is a wrapper for code you can execute.

To provide functionality to the Login page, we will create a `LoginViewModel` and bind this to `LoginPage`.

To help with creating view models, we will create a `BaseViewModel` that provides an implementation of `INotifyPropertyChanged`. 

1. In the **Visual Studio Solution Explorer**, right-click on the second-from-the-top **HappyXamDevs** option > **Add** > **New Folder**
    > **Warning:** Do not select **Add Solution Folder**. If you are given the option **Add Solution Folder**, you have right-clicked on top-most **HappyXamDevs** option.

2. In the **Visual Studio Solution Explorer**, name the new folder `ViewModels`

3. In the **Visual Studio Solution Explorer**, right-click on the newly created `ViewModels` folder > **Add** > **Class**

    - (Mac) On Visual Studio for Mac, right-click on the newly created `ViewModels` folder > **Add** > **New File**

4. (Mac) In the **New File** window, on the left-hand menu, select **General**

5. (Mac) In the **New File** window, select **Empty Class**

6. In the **Add New Item** window, name the file `BaseViewModel.cs`

7. (PC) In the **Add New Item** window, click **Add**
    - (Mac) In the **Add New Item** window, click **New**

8. In the **BaseViewModel.cs** editor, enter the following code

```cs
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HappyXamDevs.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool Set<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            field = value;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            return true;
        }
    }
}
```

> **About the Code** 
>
> This abstract base class implements the `INotifyPropertyChanged` interface which contains a single member - the `PropertyChanged` event. 
>
> `Set` is a helper method to allow you to update a field and if the value of the field changes, raise the `PropertyChanged` event. The `CallerMemberName` attribute on the `propertyName` parameter means the compiler will pass the name of the calling method or property in for you. This is allows us to call `Set` from a property setter and the compiler will automatically pass in the property name, ensuring the change notification is raised for the correct property.

9. In the **Visual Studio Solution Explorer**, right-click on the `ViewModels` folder > **Add** > **Class**

    - (Mac) On Visual Studio for Mac, right-click on the  `ViewModels` folder > **Add** > **New File**

10. In the **Add New Item** window, name the file `LoginViewModel.cs`

11. (PC) In the **Add New Item** window, click **Add**
    - (Mac) In the **Add New Item** window, click **New**

12. In the **LoginViewModel.cs** editor, enter the following code:

```csharp
using System.Threading.Tasks;
using System.Windows.Input;
using HappyXamDevs.Services;
using Xamarin.Forms;

namespace HappyXamDevs.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public LoginViewModel()
        {
            LoginCommand = new Command(async () => await Login());
        }

        public ICommand LoginCommand { get; }

        private async Task Login()
        {
            var azureService = DependencyService.Get<IAzureService>();
            if (await azureService.Authenticate())
            {
                await Application.Current.MainPage.Navigation.PopModalAsync();
            }
        }
    }
}
```

> **About the Code**
>
> `LoginViewModel` inherits from `BaseViewModel`
>
> `LoginCommand` will be used when the user taps the `LoginButton` (We will wire `LoginCommand` to `LoginButton` in the next step)
>
> `Task Login()` uses the Xamarin.Forms Dependency Service to call the platform-specific implementation of `IAzureService` and authenticate the user.

## 5. Binding the View to the ViewModel

Now we have a ViewModel, it needs to be set as the binding context of the page, and the login button needs to be wired up to the command.

1. In the **Visual Studio Solution Explorer**, open **HappyXamDevs** > **LoginPage.xaml**

2. In the `LoginPage.xaml` editor, update the code to match the following:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:ios="clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core"
             ios:Page.UseSafeArea="true"
             xmlns:viewModels="clr-namespace:HappyXamDevs.ViewModels"
             x:Class="HappyXamDevs.LoginPage">

    <ContentPage.BindingContext>
        <viewModels:LoginViewModel />
    </ContentPage.BindingContext>

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
                    Command="{Binding LoginCommand}"
                    Text="Log in with Facebook"
                    BackgroundColor="{StaticResource CoolPurple}"
                    TextColor="White" />
        </Grid>
    </ContentPage.Content>
</ContentPage>
```

> **About the Code**
>
> `xmlns:viewModels` adds the XML Namespace `viewModels`
>
> `ContentPage.BindingContext` tells the Content Page to bind to `LoginViewModel`
>
> `Command="{Binding LoginCommand}"` allows button taps to trigger `LoginViewModel.LoginCommand`

## 6. Launching the login page if the user is not logged in

The Blank Forms App template just contained one `ContentPage`, `MainPage.xaml`.

We will wrap the page in a `NavigationPage` which provides page-level navigation allowing the user to navigate back and forth. You can read more on navigation pages in the [Xamarin docs](https://docs.microsoft.com/xamarin/xamarin-forms/app-fundamentals/navigation/?WT.mc_id=mobileappsoftomorrow-workshop-jabenn).

1. In the **Visual Studio Solution Explorer**, open **HappyXamDevs** > **App.xaml.cs**

2. In the **App.xaml.cs** editor, enter the following code:

```csharp
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace HappyXamDevs
{
    public partial class App : Xamarin.Forms.Application
    {
        public App()
        {
            InitializeComponent();

            var navigationPage = new Xamarin.Forms.NavigationPage(new MainPage());

            navigationPage.BarBackgroundColor = (Color)Resources["CoolPurple"];
            navigationPage.BarTextColor = Color.White;

            navigationPage.On<iOS>().SetPrefersLargeTitles(true);
            navigationPage.On<iOS>().SetUseSafeArea(true);

            MainPage = navigationPage;
        }
    }
}
```

> **About the Code**
>
> `navigationPage.BarBackgroundColor` sets the Navigation Bar's Background Color
>
> `navigationPage.BarTextColor` sets the Navigation Bar's Text Color
>
> `navigationPage.On<iOS>().SetPrefersLargeTitles(true);` ensures [Large Titles](https://developer.apple.com/documentation/uikit/uinavigationbar/2908999-preferslargetitles) are used on iOS
>
> `navigationPage.On<iOS>().SetUseSafeArea(true);` ensures the UI doesn't get clipped by the iPhone notch
>
> `MainPage = navigationPage` sets the `MainPage`, also known as the "Root Page", of the Xamarin.Forms application to use a `NavigationPage`

3. In the **Visual Studio Solution Explorer**, open **HappyXamDevs** > **MainPage.xaml.cs**

4. In the **MainPage.xaml.cs** editor, enter the following code:

```csharp
using System.Linq;
using HappyXamDevs.Services;
using Xamarin.Forms;

namespace HappyXamDevs
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var azureService = DependencyService.Get<IAzureService>();

            if (!azureService.IsLoggedIn())
            {
                if (!Navigation.ModalStack.Any())
                    await Navigation.PushModalAsync(new LoginPage(), false);
            }
        }
    }
}
```

> **About the Code**
>
> `void OnAppearing()` is triggered every time the `ContentPage` appears on the screen when the app is running
>
> If the user has not logged in, they will be presented the `LoginPage`

## 7. Test the Login Flow

### 7a. Test the Login Flow, Android

1. In Visual Studio, right-click on **HappyXamDevs.Android** > **Set as Startup Project**

2. (PC) In Visual Studio, select **Debug** > **Start Debugging**
    - (Mac) In Visual Studio for Mac, select **Run** > **Start Debugging**

3. On the Android device, when the app launches, ensure the **LoginPage** appears

4. On the Android device, on the **LoginPage**, select **Log in with Facebook**

5. On the Android device, ensure the Facebook login flow appears

6. On the Android device, complete the Facebook login flow

7. On the Android device, ensure **LoginPage** disappears and you are returned to **MainPage**
    > **Note:** **MainPage** should say **Welcome to Xamarin.Forms**

### 7b. Test the Login Flow, UWP

1. (PC) In Visual Studio, right-click on **HappyXamDevs.UWP** > **Set as Startup Project**
    - (Mac) _Skip this step_

2. (PC) In Visual Studio, select **Debug** > **Start Debugging**
    - (Mac) _Skip this step_

3. (PC) On the UWP window, when the app launches, ensure the **LoginPage** appears

4. (PC) On the UWP window, on the **LoginPage**, select **Log in with Facebook**
    - (Mac) _Skip this step_

5. (PC) On the UWP window, ensure the Facebook login flow appears
    - (Mac) _Skip this step_

6. (PC) On the UWP window, complete the Facebook login flow
    - (Mac) _Skip this step_

7. (PC) On the UWP window, ensure **LoginPage** disappears and you are returned to **MainPage**
    > **Note:** **MainPage** should say **Welcome to Xamarin.Forms**
    - (Mac) _Skip this step_

### 7c. Test the Login Flow, iOS

1. (PC) _Skip this step_
    - (Mac) In Visual Studio, right-click on **HappyXamDevs.iOS** > **Set as Startup Project**

2. (PC) _Skip this step_
    - (Mac) In Visual Studio for Mac, select **Run** > **Start Debugging**

3. (PC) _Skip this step_
    - (Mac) On the iOS device, when the app launches, ensure the **LoginPage** appears

4. (PC) _Skip this step_
    - (Mac) On the iOS device, on the **LoginPage**, select **Log in with Facebook**

5. (PC) _Skip this step_
    - (Mac) On the iOS device, ensure the Facebook login flow appears

6. (PC) _Skip this step_
    - (Mac) On the iOS device, complete the Facebook login flow

7. (PC) _Skip this step_
    - (Mac) On the iOS device, ensure **LoginPage** disappears and you are returned to **MainPage**
    > **Note:** **MainPage** should say **Welcome to Xamarin.Forms**

## 8. Persist the login between sessions

When we run the app more than once, we are required to log in each time.

After completing the login flow, `MobileServicesClient.LoginAsync()` returns an authorization token and a user id from the Azure Function app authentication; these values can be persisted to the mobile device. By reusing the authorization token the user does not need to log in each time the app launches.

For the sake of simplicity during this workshop we will be persisting these using un-encrypted Xamarin.Forms [Application properties](https://docs.microsoft.com/dotnet/api/xamarin.forms.application.properties/?WT.mc_id=mobileappsoftomorrow-workshop-jabenn). These exist as a dictionary of strings to objects and is a property on the `Application` instance, available using the `Application.Current` static property.

> **Important:** In a production app you should NOT store these values without encryption. Instead encrypt the values using [Xamarin.Essentials.SecureStorage](https://docs.microsoft.com/xamarin/essentials/secure-storage/?WT.mc_id=mobileappsoftomorrow-workshop-jabenn).

1. In Visual Studio Solution Explorer, open **HappyXamDevs** > **Services** > **AzureServiceBase.cs**

2. In the **AzureServiceBase.cs** editor, add the following using statement:

```csharp
using Xamarin.Forms;
```

3. In the **AzureServiceBase.cs** editor, define two constant fields:

```csharp
const string AuthTokenKey = "auth-token";
const string UserIdKey = "user-id";
```

> **About the Code**
>
> `AuthTokenKey` and `UserIdKey` are unique keys that will be used to store/retrieve a key-value

4. In the **AzureServiceBase.cs** editor, add the following method:

```csharp
void TryLoadUserDetails()
{
    if (Client.CurrentUser != null)
        return;

    if (Application.Current.Properties.TryGetValue(AuthTokenKey, out var authToken) &&
        Application.Current.Properties.TryGetValue(UserIdKey, out var userId))
    {
        Client.CurrentUser = new MobileServiceUser(userId.ToString())
        {
            MobileServiceAuthenticationToken = authToken.ToString()
        };
    }
}
```

> **About the Code**
>
> In `TryLoadUserDetails()`, if a user's `AuthTokenKey` and `UserIdKey` exist, use them to initialize `MobileServicesClient.CurrentUser`

5. In the **AzureServiceBase.cs** editor, update `bool IsLoggedIn()`:

```csharp
public bool IsLoggedIn()
{
    TryLoadUserDetails();
    return Client.CurrentUser != null;
}
```

6. In the **AzureServiceBase.cs** editor, update the existing `Authenticate` method:		

  ```csharp		
 public async Task<bool> Authenticate()		
 {		
     if (IsLoggedIn())		
         return true;		
		
     try		
     {		
         await AuthenticateUser();		
     }		
     catch (InvalidOperationException)		
     {		
         return false;		
     }		
		
     if (Client.CurrentUser != null)		
     {		
         Application.Current.Properties[AuthTokenKey] = Client.CurrentUser.MobileServiceAuthenticationToken;		
         Application.Current.Properties[UserIdKey] = Client.CurrentUser.UserId;		
		
         await Application.Current.SavePropertiesAsync();		
     }		
		
     return IsLoggedIn();		
 }		
 ```

> **About the Code**
>
>`bool IsLoggedIn()` will first try to initialize `MobileServicesClient.CurrentUser` using `TryLoadUserDetails()` before checking `CurrentUser`

> **Note:** If you skipped the Facebook authentication use the following code instead:
    ```csharp
    public bool IsLoggedIn() => true;
    ```

## Next step

Now that your app can authenticate successfully, the next step is to [wire up the camera using a Xamarin plugin](./5-WireUpTheCamera.md).
