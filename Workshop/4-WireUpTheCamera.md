# Wire up the camera

The app you are building is for sharing photos of happy Xamarin developers, so it needs access to the camera and image library to allow the user to take and upload photos. The APIs to access the camera are very different on each platform, so instead of accessing them directly we will use a Xamarin Plugin.

## 1. Installing `Xam.Plugin.Media` NuGet Package

The [Xamarin Media Plugin](https://www.nuget.org/packages/Xam.Plugin.Media/) provides a simple, cross-platform abstraction over the various camera APIs for iOS, Android and UWP.

### PC

1. In Visual Studio, right-click the `HappyXamDevs` solution > **Manage NuGet Packages For Solution..**

2. In the **NuGet Package Manager** window, select **Browse**

3. In the **NuGet Package Manager** window, in the search bar, enter **Xam.Plugin.Media**

4. In the **NuGet Package Manager** window, in the search results, select **Xam.Plugin.Media**

5. In the **NuGet Package Manager** window, select **Install**

### Mac

1. In Visual Studio for Mac, right-click the `HappyXamDevs` project > **Add** > **Add NuGet Packages**

2. In the **NuGet Package Manager** window, in the search bar, enter **Xam.Plugin.Media**

3. In the **NuGet Package Manager** window, in the search results, select **Xam.Plugin.Media**

4. In the **NuGet Package Manager** window, select **Add Package**

5. Repeat these steps for the `HappyXamDevs.Android` and  `HappyXamDevs.iOS` projects

## 2. Configure iOS Camera Settings

Before you can access the camera or the photo library, you have to add a privacy description. This is shown to the used inside the dialog to allow your app to access the camera or the users photo library.

### PC

1. In the Visual Studio Solution Explorer, right-click on **HappyXamDevs.iOS** > **Info.plist** > **Open with...**

2. In the **Open With** window, select **Generic PList editor** > **OK**.

3. In the **Info.plist** editor, at the very bottom of the list of entries, click **+**

4. In the **Info.plist** editor, revel a drop-down menu by double-clicking the newly created **Custom Property**

5. In the **Info.plist** editor, in the drop-down menu, select **Privacy - Camera usage description**

6. In the **Info.plist** editor, next to **Privacy - Camera usage description**, set its **Value**:
    - **Value**: The camera is used to find Happy Xamarin Developers

7. In the **Info.plist** editor, at the very bottom of the list of entries, click **+**

8. In the **Info.plist** editor, revel a drop-down menu by double-clicking the newly created **CustomProperty**

9. In the **Info.plist** editor, in the drop-down menu, select **Privacy - Photo library usage description**

10. In the **Info.plist** editor, next to **Privacy - Photo library usage description**, set its **Value**:
    - **Value**: The photo library is used to select pictures of Happy Xamarin Developers

11. In Visual Studio, save the changes to **Info.plist** by selecting **File** > **Save All**

### Mac

1. In Visual Studio Solution Explorer, double-click **HappyXamDevs.iOS** > **Info.plist**

2. In the **Info.plist** editor, at the bottom, select **Source**

3. In the **Info.plist** editor, double-click **Add new entry**

4. In the **Info.plist** editor, revel a drop-down menu by double-clicking the newly created **Custom Property**

5. In the **Info.plist** editor, in the drop-down menu, select **Privacy - Camera usage description**

6. In the **Info.plist** editor, next to **Privacy - Camera usage description**, set its **Value**:
    - **Value**: The camera is used to find Happy Xamarin Developers

7. In the **Info.plist** editor, double-click **Add new entry**

8. In the **Info.plist** editor, revel a drop-down menu by double-clicking the newly created **CustomProperty**

9. In the **Info.plist** editor, in the drop-down menu, select **Privacy - Photo library usage description**

10. In the **Info.plist** editor, next to **Privacy - Photo library usage description**, set its **Value**: 
    - **Value**: The photo library is used to select pictures of Happy Xamarin Developers

11. In Visual Studio, save the changes to **Info.plist** by selecting **File** > **Save All**

## 2. Configure Android Camera Settings

### Configure the manifest

1. In Visual Studio Solution Explorer, open **HappyXamDevs.Android** > **Properties** > **AndroidManifest.xml**

2. If you are using Visual Studio for Mac, In the **AndroidManifest.xml** editor, at the bottom, select **Source**

3. In the **AndroidManifest.xml** source editor, update the `application` node in the XML to be the following:

   ```xml
    <application android:label="HappyXamDevs.Android">
        <provider android:name="android.support.v4.content.FileProvider" android:authorities="${applicationId}.fileprovider" android:exported="false" android:grantUriPermissions="true">
            <meta-data android:name="android.support.FILE_PROVIDER_PATHS" android:resource="@xml/file_paths">
            </meta-data>
        </provider>
    </application>
   ```

   > **About the Code**
   >
   > `<provider> android:name="android.support.v4.content.FileProvider" ` allows our app to access the Android File Provider

4. In Visual Studio, save the changes to **AndroidManifest.xml** by selecting **File** > **Save All**

### Create an XML resource folder

1. In the Visual Studio Solution Explorer, right-click on **HappyXamDevs.Android** > **Resources** > **Add** > **New Folder**

1. In the Visual Studio Solution Explorer, name the new folder `xml`

### Create an XML resource file

#### PC

1. In the Visual Studio Solution Explorer, right-click on the newly created `xml` folder > **Add** > **Class**

1. Name the file `file_paths.xml`
    > **Note:** `file_paths.xml` must be all lower-case

1. Click **New**

#### Mac

1. On Visual Studio for Mac, right-click on the newly created `xml` folder > **Add** > **New File**

1. Select **XML -> Empty XML File**

1. Name the file `file_paths.xml`
    > **Note:** `file_paths.xml` must be all lower-case

1. Click **New**

#### Add the code

1. In the **file_paths.xml** editor, enter the following code:

   ```xml
   <?xml version="1.0" encoding="utf-8"?>
   <paths xmlns:android="http://schemas.android.com/apk/res/android">
       <external-files-path name="my_images" path="Pictures" />
       <external-files-path name="my_movies" path="Movies" />
   </paths>
   ```

   > **About the Code**
   >
   > `<paths>` allows us to set an `external-files-path` for any image or video taken by the camera in our app

### Configure permissions

1. In the Visual Studio Solution Explorer, open **HappyXamDevs.Android** > **MainActivity.cs**

2. In the **MainActivity.cs** editor, add a call to `Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult` at the top of the `OnRequestPermissionsResult` method.

   ```csharp
   public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
   {
       Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
       ...
   }
   ```

3. Add the following code to call `Plugin.CurrentActivity.CrossCurrentActivity.Current.Init` in the `OnCreate` method, before the call to `LoadApplication`:

   ```csharp
   protected override void OnCreate(Bundle savedInstanceState)
   {
       ...
       Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, savedInstanceState);
       .LoadApplication(new App());
   }
   ```

   > **About the Code**
   >
   > `OnRequestPermissionsResult` allows the Xamarin Media Plugin to display Permissions Requests to current screen of our app

## 3. Configure UWP Camera Settings

1. In the Visual Studio Solution Explorer, open **HappyXamDevs.UWP** > **Package.appxmanifest**

2. In the **Package.appxmanifest** editor, select the **Capabilities** tab

3. In the **Package.appxmanifest** **Capabilities** tab, in the **Capabilities** list, check the option **Webcam**

4. In Visual Studio, save the changes to **Package.appxmanifest** by selecting **File** > **Save All**

## 4. Add some images

For the UI, you will need to add a couple of toolbar buttons to **MainPage.xaml**, one to use the camera to take a photo, the other to select a photo from the users photo library.

At this point we will just implement the code to take the photo or access the photo library.

### Android

Android stores images in the `Resources/drawable` folders, with different `drawable` folders for different device resolutions - so `drawable-hdpi` for high density screens, `drawable-xhdpi` for extra high density screens and so on. Images of different resolutions are put into these folders.

1. In the Visual Studio Solution Explorer, navigate to **HappyXamDevs.Android** > **Resources**

2. Note the many `drawable` folders
  
   >  **Note:** The lowest-resolution image will be added to `drawable-hdpi` while the highest-resolution image will be added to `drawable-xxxhdpi`

#### PC

Repeat the following steps for all the different `drawable-` folders (for example `drawable-hdpi`, `drawable-xxxhdpi`):

1. In the Visual Studio Solution Explorer, right-click on the first **drawable-** folder > **Add** > **Existing Item...**

2. In the file explorer window, open the code cloned or downloaded form GitHub, navigate to **MobileAppsOfTomorrow-Lab** > **Assets** > **Android**. Open the **drawable-** folder that matches the one in the Android project you right-clicked on.

3. In the file explorer window, select **SelectFromLibrary.png** and **TakePhoto.png**.

4. Click **Open**

Repeat this for all the **drawable-** folders.

#### Mac

Repeat the following steps for all the different `drawable-` folders (for example `drawable-hdpi`, `drawable-xxxhdpi`):

1. In the Visual Studio Solution Explorer, right-click on the first **drawable-** folder > **Add** > **Add Files...**

2. In the file explorer window, open the code cloned or downloaded form GitHub, navigate to **MobileAppsOfTomorrow-Lab** > **Assets** > **Android**. Open the **drawable-** folder that matches the one in the Android project you right-clicked on.

3. In the file explorer window, select **SelectFromLibrary.png** and **TakePhoto.png**.

4. Click **Open**

Repeat this for all the **drawable-** folders.

### iOS

iOS uses asset catalogs to manage images. For each image, we create a named image set with 3 different sizes - original size, 2x the resolution and 3x the resolution.

At runtime, the iOS device will select the appropriate image based on its screen resolution.

#### PC

1. In the Visual Studio Solution Explorer, open **HappyXamDevs.iOS** >  **Asset Catalogs** > **Assets**
2. In the **Assets.xcassets** window, select **Add** (box with green **+**) > **Add Image Set**
3. In the **Assets.xcassets** window, rename the newly created Image set from **Image** to **SelectFromLibrary**
    > **Note:** The name is case-sensitive and must contain the underscore
4. In the **SelectFromLibrary** catalog, select the first **1x** box
    > **Note:** (Mac) Be sure to select the top-most **1x** box, directly above **Universal**. We will not be using the rows below, marked **iOS** **iPad**, **Apple Watch** and **Car**.
5. In the file explorer window, navigate to **MobileAppsOfTomorrow-Lab** > **Assets** > **iOS**
6. In the file explorer, double-click **SelectFromLibrary@1x.png**
7. Repeat the above steps for the **2x** and **3x** boxes, using the **@2x** and **@3x** images respectively.

Repeat all the above steps, but for an image set called **TakePhoto**, using the **TakePhoto@_x.png** images.

#### Mac

1. In the Visual Studio Solution Explorer, open **HappyXamDevs.iOS** >  **Assets.xcassets**
2. In the **Assets.xcassets** window, on the left-hand menu, right-click **AppIcon** > **New Image Set**
3. In the **Assets.xcassets** window, rename the newly created Image set from **Image** to **SelectFromLibrary**
    > **Note:** The name is case-sensitive and must contain the underscore
4. In the **SelectFromLibrary** catalog, select the first **1x** box
    > **Note:** (Mac) Be sure to select the top-most **1x** box, directly above **Universal**. We will not be using the rows below, marked **iOS** **iPad**, **Apple Watch** and **Car**.
5. In the file explorer window, navigate to **MobileAppsOfTomorrow-Lab** > **Assets** > **iOS**
6. In the file explorer, double-click **SelectFromLibrary@1x.png**
7. Repeat the above steps for the **2x** and **3x** boxes, using the **@2x** and **@3x** images respectively.

Repeat all the above steps, but for an image set called **TakePhoto**, using the **TakePhoto@_x.png** images.

### UWP

Images are added to the root folder of the UWP project; it is not recommended to add pictures to a sub-folder for a Xamarin.Forms.UWP app.

When the application runs, the UWP framework will automatically select the best looking resolution based on the screen.

> **Note:** If using Visual Studio for Mac, skip this step

1. In the Visual Studio Solution Explorer, right-click **HappyXamDevs.UWP** > **Add** > **Existing Item...**

2. In the file explorer, navigate to **MobileAppsOfTomorrow-Lab** > **Assets** > **UWP**

3. In the file explorer, select all the **SelectFromLibrary.scale** files (**scale-100**, **scale-200** etc.)

4. Click **Add**

## 5. Adding Toolbar Buttons to `MainPage.xaml`

1. In the Visual Studio Solution Explorer, open **HappyXamDevs** > **MainPage.xaml**

2. In the **MainPage.xaml** editor, replace the following code:

    ```xml
    <?xml version="1.0" encoding="utf-8"?>
    <ContentPage 
        xmlns="http://xamarin.com/schemas/2014/forms" 
        xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml" 
        xmlns:local="clr-namespace:HappyXamDevs" 
        x:Class="HappyXamDevs.MainPage"
        Title="Happy Developers">

        <ContentPage.ToolbarItems>
            <ToolbarItem Order="Primary" Icon="TakePhoto.png" Priority="0" />
            <ToolbarItem Order="Primary" Icon="SelectFromLibrary.png" Priority="1" />
        </ContentPage.ToolbarItems>

    </ContentPage>
    ```

    > **About the Code**
    >
    > `ToolbarItem` is a button that appears in Navigation Bar

## 6. Create a base view model

Xamarin.Forms supports data binding, allowing you to wire a page up to a view model and synchronize data back and forth. To make data binding work, the view model must implement the `INotifyPropertyChanged` interface, an interface that contains an event to notify when data changes. The view will detect these changes and update what is displayed. For interactive controls such as buttons, there is a `Command` property on the control that can be bound to a command on the view model - a command being a property of type `ICommand` which is a wrapper for code you can execute.

To provide functionality to the Login page, you will need to create a `LoginViewModel` and bind this to the page.

To help with creating view models, you can create a `BaseViewModel` that provides an implementation of `INotifyPropertyChanged`.

1. In the Visual Studio Solution Explorer, right-click on the **HappyXamDevs** project > **Add** > **New Folder**
    > **Warning:** Do not select **Add Solution Folder**. If you are given the option **Add Solution Folder**, you have right-clicked on the **HappyXamDevs** solution, not the project.

1. In the Visual Studio Solution Explorer, name the new folder `ViewModels`

### PC

1. In the Visual Studio Solution Explorer, right-click on the newly created **ViewModels** folder > **Add** > **Class**

2. In the **Add New Item** window, name the file `BaseViewModel.cs`

3. In the **Add New Item** window, click **Add**

### Mac

1. On Visual Studio for Mac, right-click on the newly created `ViewModels` folder > **Add** > **New File**

2. In the **New File** window, select **General -> Empty Class**

3. Name the file `BaseViewModel.cs`

4. Click **New**

### Add the code

Add the following code to this file:

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

> **About The Code**
>
> This abstract base class implements the `INotifyPropertyChanged` interface which contains a single member - the `PropertyChanged` event. It also has a helper method to allow you to update a field and if the value of the field changes, the `PropertyChanged` event is raised.
>
> The `CallerMemberName` attribute on the `propertyName` parameter means you don't have to pass a value to this parameter and the compiler will pass the name of the calling method or property in for you. 
> 
> This is incredibly useful, you can call `Set` from a property setter and the compiler will automatically pass the property name in so that the property change notification is raised for the correct property.

### 7. Add a MainViewModel

In this section you will add behavior to your main page using a view model.

### PC

1. In the Visual Studio Solution Explorer, right-click on the newly created **ViewModels** folder > **Add** > **Class**

2. In the **Add New Item** window, name the file `MainViewModel.cs`

3. In the **Add New Item** window, click **Add**

### Mac

1. On Visual Studio for Mac, right-click on the newly created `ViewModels` folder > **Add** > **New File**

2. In the **New File** window, select **General -> Empty Class**

3. Name the file `MainViewModel.cs`

4. Click **New**

### Add the code

1. In the **MainViewModel.cs** editor, enter the following code:

    ```csharp
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Plugin.Media;
    using Plugin.Media.Abstractions;
    using Xamarin.Forms;

    namespace HappyXamDevs.ViewModels
    {
        public class MainViewModel : BaseViewModel
        {
            public MainViewModel()
            {
                TakePhotoCommand = new Command(async () => await TakePhoto());
                SelectFromLibraryCommand = new Command(async () => await SelectFromLibrary());
            }

            public ICommand SelectFromLibraryCommand { get; }
            public ICommand TakePhotoCommand { get; }

            private async Task SelectFromLibrary()
            {
                var options = new PickMediaOptions { PhotoSize = PhotoSize.Medium };
                var photo = await CrossMedia.Current.PickPhotoAsync(options);
            }

            private async Task TakePhoto()
            {
                var options = new StoreCameraMediaOptions
                {
                    PhotoSize = PhotoSize.Medium,
                    DefaultCamera = CameraDevice.Front
                };
                var photo = await CrossMedia.Current.TakePhotoAsync(options);
            }
        }
    }
    ```

    > **About the Code**
    >
    > `MainViewModel` inherits from `BaseViewModel`
    >
    > `TakePhotoCommand` runs `TakePhoto()` and will be triggered by a Button Command  (we will add this in the next step)
    >
    > `SelectFromLibraryCommand` runs `SelectFromLibrary()` and will be triggered by a Button Command (we will add this in the next step)

2. In the Visual Studio Solution Explorer, open **HappyXamDevs** > **MainPage.xaml**

3. In the **MainPage.xaml** editor, replace the following code:

    ```xml
    <?xml version="1.0" encoding="utf-8"?>
    <ContentPage 
        xmlns="http://xamarin.com/schemas/2014/forms" 
        xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml" 
        xmlns:local="clr-namespace:HappyXamDevs" 
        xmlns:viewModels="clr-namespace:HappyXamDevs.ViewModels"
        x:Class="HappyXamDevs.MainPage"
        Title="Happy Developers">

        <ContentPage.BindingContext>
            <viewModels:MainViewModel/>
        </ContentPage.BindingContext>

        <ContentPage.ToolbarItems>
            <ToolbarItem Order="Primary"
                    Icon="TakePhoto.png"
                    Priority="0"
                    Command="{Binding TakePhotoCommand}"/>
            <ToolbarItem Order="Primary"
                    Icon="SelectFromLibrary.png"
                    Priority="1"
                    Command="{Binding SelectFromLibraryCommand}"/>
        </ContentPage.ToolbarItems>

    </ContentPage>
    ```

    > **About the Code**
    >
    > `xmlns:viewModels="clr-namespace:HappyXamDevs.ViewModels"` adds the XML Namespace `HappyXamDevs.ViewModels`
    >
    > `<ContentPage.BindingContext>` tells the view, `MainPage.xaml`, to be bound to a ViewModel, `MainViewModel.cs`
    >
    > `Command="{Binding TakePhotoCommand}"` will trigger `MainViewModel.TakePhotoCommand` any time the TakePhoto ToolbarItem is tapped
    >
    > `Command="{Binding SelectFromLibraryCommand}"` will trigger `MainViewModel.SelectFromLibraryCommand` any time the SelectFromLibrary ToolbarItem is tapped

## 8. Test Camera & Photo Library Functionality

### Android

1. In Visual Studio, right-click on **HappyXamDevs.Android** > **Set as Startup Project**

2. - (PC) In Visual Studio, select **Debug** > **Start Debugging**
    - (Mac) In Visual Studio for Mac, select **Run** > **Start Debugging**

3. On the Android device, on the **MainPage**, tap the Camera icon

4. On the Android device, if prompted for permission, tap **Allow**

5. On the Android device, ensure the Camera appears

6. On the Android device, tap the back button to navigate away from the Camera

7. On the Android device, on **MainPage**, tap files icon

8. On the Android device, if prompted for permission, tap **Allow**

9. On the Android device, on **MainPage**, ensure the photos library appears

### UWP

UWP only works on Windows, so if you are using Visual Studio for Mac, skip this step.

1. In Visual Studio, right-click on **HappyXamDevs.UWP** > **Set as Startup Project**

2. In Visual Studio, select **Debug** > **Start Debugging**

3. On the UWP device, on the **MainPage**, tap the Camera icon

4. On the UWP device, if prompted for permission, tap **Allow**

5. On the UWP device, ensure the Camera appears

6. On the UWP device, tap the back button to navigate away from the Camera

7. On the UWP device, on **MainPage**, tap files icon

8. On the UWP device, if prompted for permission, tap **Allow**

9. On the UWP device, on **MainPage**, ensure the photos library appears

### iOS

> The camera can only be tested on a physical iOS device because the iOS Simulator does not have support for the camera

1. In Visual Studio, right-click on **HappyXamDevs.UWP** > **Set as Startup Project**

2. - (PC) In Visual Studio, select **Debug** > **Start Debugging**
    - (Mac) In Visual Studio for Mac, select **Run** > **Start Debugging**

3. On the iOS device, on the **MainPage**, tap the Camera icon

4. On the iOS device, if prompted for permission, tap **Allow**

5. On the iOS device, ensure the Camera appears

6. On the iOS device, tap the back button to navigate away from the Camera

7. On the iOS device, on **MainPage**, tap files icon

8. On the iOS device, if prompted for permission, tap **Allow**

9. On the iOS device, on **MainPage**, ensure the photos library appears

## Next step

Now that your app can take photos, the next step is to [detect faces and emotion in these photos using the Azure FaceAPI](./5-DetectFaces.md).
