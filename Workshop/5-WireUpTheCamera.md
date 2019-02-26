# Wire up the camera

The app you are building is for sharing photos of happy Xamarin developers, so it needs access to the camera and image library to allow the user to take and upload photos. The APIs to access the camera are very different on each platform, so instead of accessing them directly we will use a Xamarin Plugin.

## 1. Install Xam.Plugin.Media NuGet Package

The [Xamarin Media Plugin](https://www.nuget.org/packages/Xam.Plugin.Media/) provides a simple, cross-platform abstraction over the various camera APIs for iOS, Android and UWP.

1. Open the Xamarin.Forms app in Visual Studio

    > **Note:** The completed app from Section 1 is available in **FinishedWorkshopSteps** > **1-CreateSolution**

2. (PC) In Visual Studio, right-click the `HappyXamDevs` solution > **Manage NuGet Packages For Solution..**

    - (Mac) In Visual Studio for Mac, right-click the `HappyXamDevs` project > **Add** > **Add NuGet Packages**

3. (PC) In the **NuGet Package Manager** window, select **Browse**

    - (Mac) _Skip this step_

4. In the **NuGet Package Manager** window, in the search bar, enter **Xam.Plugin.Media**

5. In the **NuGet Package Manager** window, in the search results, select **Xam.Plugin.Media**

6. (PC) In the **NuGet Package Manager** window, select **Install**

    - (Mac) In the **NuGet Package Manager** window, select **Add Package**

7. (PC) _Skip this step_

    - (Mac) In Visual Studio for Mac, right-click the `HappyXamDevs.Android` project > **Add** > **Add NuGet Packages**

8. (PC) _Skip this step_

    - (Mac) In the **NuGet Package Manager** window, in the search results, select **Xam.Plugin.Media**

9. (PC) _Skip this step_

    - (Mac) In the **NuGet Package Manager** window, select **Add Package**

10. (PC) _Skip this step_

    - (Mac) In Visual Studio for Mac, right-click the `HappyXamDevs.iOS` project > **Add** > **Add NuGet Packages**

11. (PC) _Skip this step_

    - (Mac) In the **NuGet Package Manager** window, in the search results, select **Xam.Plugin.Media**

12. (PC) _Skip this step_

    - (Mac) In the **NuGet Package Manager** window, select **Add Package**

## 2. Configure iOS Camera Settings

1. (PC) In Visual Studio Solution Explorer, right-click on **HappyXamDevs.iOS** > **Info.plist** > **Open with...**
    - (Mac) In Visual Studio Solution Explorer, double-click **HappyXamDevs.iOS** > **Info.plist**

2. (PC) In the **Open With** window, select **Generic PList editor** > **OK**.
    - (Mac) In the **Info.plist** editor, at the bottom, select **Source**

3. (PC) In the **Info.plist** editor, at the very bottom of the list of entries, click **+**
    - (Mac) In the **Info.plist** editor, double-click **Add new entry**

4. In the **Info.plist** editor, revel a drop-down menu by double-clicking the newly created **Custom Property**

5. In the **Info.plist** editor, in the drop-down menu, select **Privacy - Camera usage description**

6. In the **Info.plist** editor, next to **Privacy - Camera usage description**, set its **Value**:
    - **Value**: The camera is used to find Happy Xamarin Developers

7. (PC) In the **Info.plist** editor, at the very bottom of the list of entries, click **+**
    - (Mac) In the **Info.plist** editor, double-click **Add new entry**

8. In the **Info.plist** editor, revel a drop-down menu by double-clicking the newly created **CustomProperty**

9. In the **Info.plist** editor, in the drop-down menu, select **Privacy - Photo library usage description**

10. In the **Info.plist** editor, next to **Privacy - Photo library usage description**, set its **Value**: 
    - **Value**: The photo library is used to select pictures of Happy Xamarin Developers

11. In Visual Studio, save the changes to **Info.plist** by selectiong **File** > **Save All**

## 2. Configure Android Camera Settings

1. In Visual Studio Solution Explorer, open **HappyXamDevs.Android** > **Properties** > **AndroidManifest.xml**

2. (PC) _Skip this step_
    - (Mac) In the **AndroidManifest.xml** editor, at the bottom, select **Source**

3. In the **AndroidManifest.xml** source editor, enter the following code:

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
        <provider android:name="android.support.v4.content.FileProvider" android:authorities="${applicationId}.fileprovider" android:exported="false" android:grantUriPermissions="true">
            <meta-data android:name="android.support.FILE_PROVIDER_PATHS" android:resource="@xml/file_paths">
            </meta-data>
        </provider>
    </application>
</manifest>
```

> **About the Code**
>
> `<provider> android:name="android.support.v4.content.FileProvider" ` allows our app to access the Android File Provider

4. In Visual Studio, save the changes to **AndroidManifest.xml** by selecting **File** > **Save All**

5. In the Visual Studio Solution Explorer, right-click on the `HappyXamDevs` project > **Add** > **New Folder**

6. In the Visual Studio Solution Explorer, name the new folder `xml`

7. In the Visual Studio Solution Explorer, right-click on the newly created `xml` folder > **Add** > **Class**

    - (Mac) On Visual Studio for Mac, right-click on the newly created `xml` folder > **Add** > **New File**

8. In the **Add New Item** window, name the file `file_paths.xml`
    > **Note:** `file_paths.xml` must be all lower-case

9. In the **file_paths.xml** editor, enter the following code:

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

10. In the Visual Studio Solution Explorer, open **HappyXamDevs.Android** > **MainActivity.cs**

11. In the **MainActivity.cs** editor, add the following method:

```csharp
public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
{
    Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
    base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
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

## 4. Import SelectFromLibrary.png

For the UI, you will need to add a couple of toolbar buttons to **MainPage.xaml**, one to use the camera to take a photo, the other to select a photo from the users photo library. 

At this point we will just implement the code to take the photo or access the photo library.

### 4a. Import SelectFromLibrary.png, Android

Android stores images in the `Resources/drawable` folders, with different `drawable` folders for different device resolutions - so `drawable-hdpi` for high density screens, `drawable-xhdpi` for extra high density screens and so on. Images of different resolutions are put into these folders.

1. In the Visual Studio Solution Explorer, navigate to **HappyXamDevs.Android** > **Resources**

2. In the Visual Studio Solution Explorer, note the many `drawable` folders
    >  **Note:** The lowest-resolution image will be added to `drawable-hdpi` while the highest-resolution image will be added to `drawable-xxxhdpi`

3. (PC) In the Visual Studio Solution Explorer, right-click on the  **drawable-hdpi** folder > **Add** > **Existing Item...**

    - (Mac) In the Visual Studio Solution Explorer, right-click on  **drawable-hdpi** > **Add** > **Add Files...**

4. In the file explorer window, navigate to **MobileAppsOfTomorrow-Lab** > **Assets** > **Android** > **drawable-hdpi**

5. In the file explorer window, double-click **SelectFromLibrary.png**

6. (PC) _Skip this step_
    - (Mac) In the confirmation popup, select **Copy the file to the directory** > **OK**

7. (PC) In the Visual Studio Solution Explorer, right-click on the  **drawable-xhdpi** folder > **Add** > **Existing Item...**

    - (Mac) In the Visual Studio Solution Explorer, right-click on  **drawable-xhdpi** > **Add** > **Add Files...**

8. In the file explorer window, navigate to **MobileAppsOfTomorrow-Lab** > **Assets** > **Android** > **drawable-xhdpi**

9. In the file explorer window, double-click **SelectFromLibrary.png**

10. (PC) _Skip this step_
    - (Mac) In the confirmation popup, select **Copy the file to the directory** > **OK**

11. (PC) In the Visual Studio Solution Explorer, right-click on the  **drawable-xxhdpi** folder > **Add** > **Existing Item...**

    - (Mac) In the Visual Studio Solution Explorer, right-click on  **drawable-xxhdpi** > **Add** > **Add Files...**

12. In the file explorer window, navigate to **MobileAppsOfTomorrow-Lab** > **Assets** > **Android** > **drawable-xxhdpi**

13. In the file explorer window, double-click **SelectFromLibrary.png**

14. (PC) _Skip this step_
    - (Mac) In the confirmation popup, select **Copy the file to the directory** > **OK**

15. (PC) In the Visual Studio Solution Explorer, right-click on the  **drawable-xxxhdpi** folder > **Add** > **Existing Item...**

    - (Mac) In the Visual Studio Solution Explorer, right-click on  **drawable-xxxhdpi** > **Add** > **Add Files...**

16. In the file explorer window, navigate to **MobileAppsOfTomorrow-Lab** > **Assets** > **Android** > **drawable-xxxhdpi**

17. In the file explorer window, double-click **SelectFromLibrary.png**

18. (PC) _Skip this step_
    - (Mac) In the confirmation popup, select **Copy the file to the directory** > **OK**

### 4b. Import SelectFromLibrary.png, iOS

iOS uses asset catalogs to manage images. For each image, we create a named image set with 3 different sizes - original size, 2x the resolution and 3x the resolution.

At runtime, the iOS device will select the appropriate image based on its screen resolution.

1. (PC) In the Visual Studio Solution Explorer, open **HappyXamDevs.iOS** >  **Asset Catalogs** > **Assets**
    - (Mac) In the Visual Studio Solution Explorer, open **HappyXamDevs.iOS** >  **Assets.xcassets**

2. (PC) In the **Assets.xcassets** window, select **Add** (box with green **+**) > **Add Image Set**
    - (Mac) In the **Assets.xcassets** window, on the left-hand menu, right-click **AppIcon** > **New Image Set**

3. In the **Assets.xcassets** window, rename the newly created Image set from **Images** to **SelectFromLibrary**
    > **Note:** The name is case-sensitive and must contain the underscore

4. In the **SelectFromLibrary** catalog, select the first **1x** box
    > **Note:** (Mac) Be sure to select the top-most **1x** box, directly above **Universal**. We will not be using the rows below, marked **iOS** **iPad**, **Apple Watch** and **Car**.

5. In the file explorer window, navigate to **MobileAppsOfTomorrow-Lab** > **Assets** > **iOS**

6. In the file explorer, double-click **SelectFromLibrary@1x.png**

7. In the **SelectFromLibrary** catalog, select the first **2x** box
    > **Note:** (Mac) Be sure to select the top-most **2x** box, directly above **Universal**. We will not be using the rows below, marked **iOS** **iPad**, **Apple Watch** and **Car**.

8. In the file explorer window, navigate to **MobileAppsOfTomorrow-Lab** > **Assets** > **iOS**

9. In the file explorer, double-click **SelectFromLibrary@3x.png**

10. In the **SelectFromLibrary** catalog, select the first **3x** box
    > **Note:** (Mac) Be sure to select the top-most **3x** box, directly above **Universal**. We will not be using the rows below, marked **iOS** **iPad**, **Apple Watch** and **Car**.

11. In the file explorer window, navigate to **MobileAppsOfTomorrow-Lab** > **Assets** > **iOS**

12. In the file explorer, double-click **SelectFromLibrary@3x.png**

13. In Visual Studio, save the **Assesst.xcassets** file by selecting **File** > **Save**

### 4c. Import SelectFromLibrary.png, UWP

Images are added to the root folder of the UWP project; it is not recommended to add pictures to a sub-folder for a Xamarin.Forms.UWP app.

When the application runs, the UWP framework will automatically select the best looking resolution based on the screen.

> **Note:** If using Visual Studio for Mac, skip this step

1. In the Visual Studio Solution Explorer, right-click **HappyXamDevs.UWP** > **Add** > **Existing Item...**

2. In the file explorer, navigate to **MobileAppsOfTomorrow-Lab** > **Assets** > **UWP**

3. In the file explorer, double-click **SelectFromLibrary.scale-100**

4. In the Visual Studio Solution Explorer, right-click **HappyXamDevs.UWP** > **Add** > **Existing Item...**

5. In the file explorer, navigate to **MobileAppsOfTomorrow-Lab** > **Assets** > **UWP**

6. In the file explorer, double-click **SelectFromLibrary.scale-200**

7. In the Visual Studio Solution Explorer, right-click **HappyXamDevs.UWP** > **Add** > **Existing Item...**

8. In the file explorer, navigate to **MobileAppsOfTomorrow-Lab** > **Assets** > **UWP**

9. In the file explorer, double-click **SelectFromLibrary.scale-300**

## 5. Import TakePhoto.png

For the UI, you will need to add a couple of toolbar buttons to **MainPage.xaml**, one to use the camera to take a photo, the other to select a photo from the users photo library. 

At this point we will just implement the code to take the photo or access the photo library.

### 5a. Import TakePhoto.png, Android

Android stores images in the `Resources/drawable` folders, with different `drawable` folders for different device resolutions - so `drawable-hdpi` for high density screens, `drawable-xhdpi` for extra high density screens and so on. Images of different resolutions are put into these folders.

1. In the Visual Studio Solution Explorer, navigate to **HappyXamDevs.Android** > **Resources**

2. In the Visual Studio Solution Explorer, note the many `drawable` folders
    >  **Note:** The lowest-resolution image will be added to `drawable-hdpi` while the highest-resolution image will be added to `drawable-xxxhdpi`

3. (PC) In the Visual Studio Solution Explorer, right-click on the  **drawable-hdpi** folder > **Add** > **Existing Item...**

    - (Mac) In the Visual Studio Solution Explorer, right-click on  **drawable-hdpi** > **Add** > **Add Files...**

4. In the file explorer window, navigate to **MobileAppsOfTomorrow-Lab** > **Assets** > **Android** > **drawable-hdpi**

5. In the file explorer window, double-click **TakePhoto.png**

6. (PC) _Skip this step_
    - (Mac) In the confirmation popup, select **Copy the file to the directory** > **OK**

7. (PC) In the Visual Studio Solution Explorer, right-click on the  **drawable-xhdpi** folder > **Add** > **Existing Item...**

    - (Mac) In the Visual Studio Solution Explorer, right-click on  **drawable-xhdpi** > **Add** > **Add Files...**

8. In the file explorer window, navigate to **MobileAppsOfTomorrow-Lab** > **Assets** > **Android** > **drawable-xhdpi**

9. In the file explorer window, double-click **TakePhoto.png**

10. (PC) _Skip this step_
    - (Mac) In the confirmation popup, select **Copy the file to the directory** > **OK**

11. (PC) In the Visual Studio Solution Explorer, right-click on the  **drawable-xxhdpi** folder > **Add** > **Existing Item...**

    - (Mac) In the Visual Studio Solution Explorer, right-click on  **drawable-xxhdpi** > **Add** > **Add Files...**

12. In the file explorer window, navigate to **MobileAppsOfTomorrow-Lab** > **Assets** > **Android** > **drawable-xxhdpi**

13. In the file explorer window, double-click **TakePhoto.png**

14. (PC) _Skip this step_
    - (Mac) In the confirmation popup, select **Copy the file to the directory** > **OK**

15. (PC) In the Visual Studio Solution Explorer, right-click on the  **drawable-xxxhdpi** folder > **Add** > **Existing Item...**

    - (Mac) In the Visual Studio Solution Explorer, right-click on  **drawable-xxxhdpi** > **Add** > **Add Files...**

16. In the file explorer window, navigate to **MobileAppsOfTomorrow-Lab** > **Assets** > **Android** > **drawable-xxxhdpi**

17. In the file explorer window, double-click **TakePhoto.png**

18. (PC) _Skip this step_
    - (Mac) In the confirmation popup, select **Copy the file to the directory** > **OK**

### 5b. Import TakePhoto.png, iOS

iOS uses asset catalogs to manage images. For each image, we create a named image set with 3 different sizes - original size, 2x the resolution and 3x the resolution.

At runtime, the iOS device will select the appropriate image based on its screen resolution.

1. (PC) In the Visual Studio Solution Explorer, open **HappyXamDevs.iOS** >  **Asset Catalogs** > **Assets**
    - (Mac) In the Visual Studio Solution Explorer, open **HappyXamDevs.iOS** >  **Assets.xcassets**

2. (PC) In the **Assets.xcassets** window, select **Add** (box with green **+**) > **Add Image Set**
    - (Mac) In the **Assets.xcassets** window, on the left-hand menu, right-click **AppIcon** > **New Image Set**

3. In the **Assets.xcassets** window, rename the newly created Image set from **Images** to **TakePhoto**
    > **Note:** The name is case-sensitive and must contain the underscore

4. In the **TakePhoto** catalog, select the first **1x** box
    > **Note:** (Mac) Be sure to select the top-most **1x** box, directly above **Universal**. We will not be using the rows below, marked **iOS** **iPad**, **Apple Watch** and **Car**.

5. In the file explorer window, navigate to **MobileAppsOfTomorrow-Lab** > **Assets** > **iOS**

6. In the file explorer, double-click **TakePhoto@1x.png**

7. In the **TakePhoto** catalog, select the first **2x** box
    > **Note:** (Mac) Be sure to select the top-most **2x** box, directly above **Universal**. We will not be using the rows below, marked **iOS** **iPad**, **Apple Watch** and **Car**.

8. In the file explorer window, navigate to **MobileAppsOfTomorrow-Lab** > **Assets** > **iOS**

9. In the file explorer, double-click **TakePhoto@3x.png**

10. In the **TakePhoto** catalog, select the first **3x** box
    > **Note:** (Mac) Be sure to select the top-most **3x** box, directly above **Universal**. We will not be using the rows below, marked **iOS** **iPad**, **Apple Watch** and **Car**.

11. In the file explorer window, navigate to **MobileAppsOfTomorrow-Lab** > **Assets** > **iOS**

12. In the file explorer, double-click **TakePhoto@3x.png**

13. In Visual Studio, save the **Assesst.xcassets** file by selecting **File** > **Save**

### 5c. Import TakePhoto.png, UWP

Images are added to the root folder of the UWP project; it is not recommended to add pictures to a sub-folder for a Xamarin.Forms.UWP app.

When the application runs, the UWP framework will automatically select the best looking resolution based on the screen.

> **Note:** If using Visual Studio for Mac, skip this step

1. In the Visual Studio Solution Explorer, right-click **HappyXamDevs.UWP** > **Add** > **Existing Item...**

2. In the file explorer, navigate to **MobileAppsOfTomorrow-Lab** > **Assets** > **UWP**

3. In the file explorer, double-click **TakePhoto.scale-100**

4. In the Visual Studio Solution Explorer, right-click **HappyXamDevs.UWP** > **Add** > **Existing Item...**

5. In the file explorer, navigate to **MobileAppsOfTomorrow-Lab** > **Assets** > **UWP**

6. In the file explorer, double-click **TakePhoto.scale-200**

7. In the Visual Studio Solution Explorer, right-click **HappyXamDevs.UWP** > **Add** > **Existing Item...**

8. In the file explorer, navigate to **MobileAppsOfTomorrow-Lab** > **Assets** > **UWP**

9. In the file explorer, double-click **TakePhoto.scale-300**

## 6. Add Toolbar Buttons to MainPage.xaml

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
> `ToolbarItem` is a button that appears in Naviagtion Bar

### 7. Add a MainViewModel

In the previous section you added behavior to your login page using a view model, and you will need to do the same for this page.

1. In the Visual Studio Solution Explorer, right-click on **HappyXamDevs** > **ViewModels** > **Add** > **Class**

    - (Mac) On Visual Studio for Mac, right-click on **HappyXamDevs** > **ViewModels** > **Add** > **New File**

2. In the **Add New Item** window, name the file `MainViewModel.cs`

3. In the **MainViewModel.cs** editor, enter the following code: 

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
        public ICommand SelectFromLibraryCommand { get; }

        public ICommand TakePhotoCommand { get; }

        public MainViewModel()
        {
            TakePhotoCommand = new Command(async () => await TakePhoto());
            SelectFromLibraryCommand = new Command(async () => await SelectFromLibrary());
        }

        private async Task SelectFromLibrary()
        {
            var options = new PickMediaOptions { PhotoSize = PhotoSize.Medium };
            var photo = await CrossMedia.Current.PickPhotoAsync(options);
        }

        private async Task TakePhoto()
        {
            var options = new StoreCameraMediaOptions { PhotoSize = PhotoSize.Medium };
            var photo = await CrossMedia.Current.TakePhotoAsync(options);
        }
    }
}
```

> **About the Code**
>
> `MainViewModel` inherits from `BaseViewModel`
>
> `TakePhotoCommand` runs `TakePhoto()` and will be triggered by a Button Command (we will add this in the next step)
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
> `<ContentPage.BindingContext>` tells the view, `MainPage.xaml`, to be bound to a ViewModel, `MainViewModel.cs`
> `Command="{Binding TakePhotoCommand}"` will trigger `MainViewModel.TakePhotoCommand` any time the TakePhoto ToolbarItem is tapped
> `Command="{Binding SelectFromLibraryCommand}"` will trigger `MainViewModel.SelectFromLibraryCommand` any time the SelectFromLibrary ToolbarItem is tapped

## 8. Test Camera & Photo Library Functionality

> The iOS Simulator does not have support for the camera, so if you use the take photo button your app will crash. In a production-quality app you would need to handle this. The media plugin had methods to check to see if the camera and photo library is supported and you could show or hide the buttons based off these values.

### 8a. Test Camera & Photo Library Functionality, Android

1. In Visual Studio, right-click on **HappyXamDevs.Android** > **Set as Startup Project**

2. (PC) In Visual Studio, select **Debug** > **Start Debugging**
    - (Mac) In Visual Studio for Mac, select **Run** > **Start Debugging**

3. On the Android device, if the **LoginPage** complete the login flow

4. On the Android device, on the **MainPage**, tap the Camera icon

5. On the Android device, if prompted for permission, tap **Allow**

5. On the Android device, ensure the Camera appears

6. On the Android device, tap the back button to navigate away from the Camera

7. On the Android device, on **MainPage**, tap files icon

8. On the Android device, if prompted for permission, tap **Allow**

9. On the Android device, on **MainPage**, ensure the photos library appears

### 8b. Test Camera & Photo Library Functionality, UWP

1. In Visual Studio, right-click on **HappyXamDevs.UWP** > **Set as Startup Project**

2. (PC) In Visual Studio, select **Debug** > **Start Debugging**
    - (Mac) In Visual Studio for Mac, select **Run** > **Start Debugging**

3. On the Android device, if the **LoginPage** complete the login flow

4. On the Android device, on the **MainPage**, tap the Camera icon

5. On the Android device, if prompted for permission, tap **Allow**

5. On the Android device, ensure the Camera appears

6. On the Android device, tap the back button to navigate away from the Camera

7. On the Android device, on **MainPage**, tap files icon

8. On the Android device, if prompted for permission, tap **Allow**

9. On the Android device, on **MainPage**, ensure the photos library appears

## Next step

Now that your app can take photos, the next step is to [detect faces and emotion in these photos using the Azure FaceAPI](./6-DetectFaces.md).