# Wire up the camera

The app you are building is for sharing photos of happy Xamarin developers, so it needs access to the camera and image library to allow the user to take and upload photos. The APIs to access the camera are very different on each platform, so instead of accessing them directly we will use a Xamarin Plugin.

## Installing the Xamarin Media plugin

The [Xamarin Media Plugin](https://www.nuget.org/packages/Xam.Plugin.Media/) provides a simple, cross-platform abstraction over the various camera APIs for iOS, Android and UWP.

1. Install this plugin into all projects in the solution using the "Xam.Plugin.Media" NuGet package.
2. For iOS, when your app first access the camera or photo library, the user will be asked for their permission. iOS requires you to give a reason for asking for this permission inside your apps `info.plist` file.
    * On Visual Studio 2017 on Windows, right-click on the `info.plist` file and select _Open with..._. Select _Generic PList editor_ and click "OK". Click the + at the very bottom of the list of entries. Double click the _CustomProperty_ name that was just added and select _Privacy - Camera usage description_ from the dropdown in the first column. Set the _Value_ to be why you want to access the users camera, something like "Need to use the camera to photo happy Xamarin developers". Repeat these steps for _Privacy - Photo library usage description_.
    * On Visual Studio for Mac, double-click on the `info.plist` file. Select the _Source_ tab. Double click on the _Add new entry_ option at the bottom, and edit the Property, selecting  _Privacy - Camera usage description_ from the dropdown. Set the _Value_ to be why you want to access the users camera, something like "Need to use the camera to photo happy Xamarin developers". Repeat this for _Privacy - Photo library usage description_.

3. For Android there are multiple steps to enable permissions as well as be able to save the images to a file.
    * Open the `AndroidManifest.xml` file from the `Properties` node in the Android application. Inside the `Application` node add the following.

        ```xml
        <provider android:name="android.support.v4.content.FileProvider"
                  android:authorities="com.companyname.HappyXamDevs.fileprovider"
                  android:exported="false"
                  android:grantUriPermissions="true">
            <meta-data android:name="android.support.FILE_PROVIDER_PATHS"
                       android:resource="@xml/file_paths"></meta-data>
        </provider>
        ```

    * In the `manifest` node of the `AndroidManifest.xml` file (first line of the file), check the value of the `package` attribute. If this value differs from the `android:authorities` attribute of the `provider` node that you just added, you need to change the `android:authorities` attribute value to match. For example, in the image below we use `com.microsoft.HappyXamDevs` in both location.

        ![Manifest with custom package and android:authorities attributes](../Images/AndroidManifest.png)

    * Add a new folder to the `Resources` folder in the Android app called `xml`. Add a new file to this folder called `file_paths.xml` with the following contents.

        ```xml
        <?xml version="1.0" encoding="utf-8"?>
        <paths xmlns:android="http://schemas.android.com/apk/res/android">
            <external-files-path name="my_images" path="Pictures" />
            <external-files-path name="my_movies" path="Movies" />
        </paths>
        ```

    * The media plugin has a dependency on a permissions plugin that handles Android permission request. Add the following override of `OnRequestPermissionsResult` to the `MainActivity` class to handle any such permission requests.

        ```cs
        public override void OnRequestPermissionsResult(int requestCode,
                                                        string[] permissions,
                                                        [GeneratedEnum] Permission[] grantResults)
        {
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        ```

4. For UWP, double-click on the `Package.appxmanifest` file. Head to the _Capabilities_ tab and check the _Webcam_ option in the list.

## Wire up the UI

For the UI, you will need to add a couple of toolbar buttons to the main page, one to use the camera to take a photo and upload it, the other to select a photo from the users photo library and upload that. At this point we will just implement the code to take the photo or access the photo library.

### Adding toolbar buttons

1. Add the `SelectFromLibrary` and `TakePhoto` assets from the `Assets` folder. Refer to te previous section on [Adding an image to the login page](./4-CreateLoginPage.md/#adding-an-image-to-the-login-page) if you need guidance on how to do this
    * Add the images from the `Android` assets folder to the relevant `drawable` folders in the Android app.
    * Add image sets to the iOS app, one called `SelectFromLibrary` and one called `TakePhoto` using the relevant @1x, @2x and @3x assets.
    * Add the images to the UWP app in the root of the project, using the Assets named `SelectFromLibrary.scale-100.png`, `SelectFromLibrary.scale-200.png`, `SelectFromLibrary.scale-300.png`, `TakePhoto.scale-100.png`, `TakePhoto.scale-200.png` and `TakePhoto.scale-300.png`.

2. Now you need to add the toolbar buttons to the main page. Open the `MainPage.xaml` file and add the following to the top-level `ContentPage` to add two toolbar items using the new icons.

    ```cs
     <ContentPage.ToolbarItems>
        <ToolbarItem Order="Primary"
                     Icon="TakePhoto.png"
                     Priority="0"/>
        <ToolbarItem Order="Primary"
                     Icon="SelectFromLibrary.png"
                     Priority="1"/>
    </ContentPage.ToolbarItems>
    ```

3. To make the toolbar look a bit better, you can set the title of the main page so that the toolbar shows this title as well as the buttons. Set the `Title` attribute on the top-level `ContentPage` element to be `"Happy Developers"`.

    ```cs
    Title="Happy Developers"
    ```

### Adding a view model

In the previous section you added behavior to your login page using a view model, and you will need to do the same for this page.

1. Create a new class in the `ViewModels` folder of the `HappyXamDevs` project called `MainViewModel` and derive this class from `BaseViewModel`.

    ```cs
    public class MainViewModel : BaseViewModel
    {
    }
    ```

2. Open the `MainPage.xaml` and add an XML namespace to the top level `ContentPage` for the `HappyXamDevs.ViewModels` namespace.

    ```xml
    xmlns:viewModels="clr-namespace:HappyXamDevs.ViewModels"
    ```

3. Set up the view model as the binding context for the page.

    ```xml
    <ContentPage.BindingContext>
        <viewModels:MainViewModel/>
    </ContentPage.BindingContext>
    ```

### Taking a photo

1. Add a read-only command property to the `MainViewModel` called `TakePhotoCommand`. You will need to add a using directive for the `System.Windows.Input` namespace.

    ```cs
    public ICommand TakePhotoCommand { get; }
    ```

2. Add a constructor to the class and in this constructor, initialize this property with a new Xamarin.Forms `Command`, implemented with a new asynchronous method called `TakePhoto`. You will need to add a using directive for the `Xamarin.Forms` namespace.

    ```cs
    public MainViewModel()
    {
        TakePhotoCommand = new Command(async () => await TakePhoto());
    }

    async Task TakePhoto()
    {
    }
    ```

3. In the `TakePhoto` method, use the media plugin to launch the camera and take a photo. You can configure options around taking the photo, so to save network bandwidth you can reduce the size of the photo to 50% of what comes out of the camera - after all modern phones have cameras of a much higher resolution than the screen so you don't need a full resolution image. You will need to add using directives for `Plugin.Media` and `Plugin.Media.Abstractions`.

    ```cs
    async Task TakePhoto()
    {
        var options = new StoreCameraMediaOptions { PhotoSize = PhotoSize.Medium };
        var photo = await CrossMedia.Current.TakePhotoAsync(options);
    }
    ```

4. Wire up this command to the take photo toolbar button in the `MainPage.xaml` by binding the `Command` property.

    ```xml
    <ToolbarItem Order="Primary"
                 Icon="TakePhoto.png"
                 Priority="0"
                 Command="{Binding TakePhotoCommand}"/>
    ```

### Accessing the users photo library

1. Add a read-only command property to the `MainViewModel` called `SelectFromLibraryCommand`.

    ```cs
    public ICommand SelectFromLibraryCommand { get; }
    ```

2. In the constructor, initialize this property with a new Xamarin.Forms `Command`, implemented with a new asynchronous method called `SelectFromLibrary`.

    ```cs
    public MainViewModel()
    {
        ...
        SelectFromLibraryCommand = new Command(async () => await SelectFromLibrary());
    }

    async Task SelectFromLibrary()
    {
    }
    ```

3. In the `SelectFromLibrary` method, use the media plugin to launch the users photo library and select a photo.

    ```cs
    async Task SelectFromLibrary()
    {
        var options = new PickMediaOptions { PhotoSize = PhotoSize.Medium };
        var photo = await CrossMedia.Current.PickPhotoAsync(options);
    }
    ```

4. Wire up this command to the select from library toolbar button in the `MainPage.xaml` by binding the `Command` property.

    ```xml
    <ToolbarItem Order="Primary"
                 Icon="SelectFromLibrary.png"
                 Priority="1"
                 Command="{Binding SelectFromLibraryCommand}"/>
    ```

### Test out your code

Launch one of the app projects to see the camera and photo library buttons working.

> The iOS Simulator does not have support for the camera, so if you use the take photo button your app will crash. In a production-quality app you would need to handle this. The media plugin had methods to check to see if the camera and photo library is supported and you could show or hide the buttons based off these values.

## Next step

Now that your app can take photos, the next step is to [detect faces and emotion in these photos using the Azure FaceAPI](./6-DetectFaces.md).