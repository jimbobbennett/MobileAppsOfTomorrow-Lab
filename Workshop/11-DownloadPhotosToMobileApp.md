# Downloading the photos to the mobile app

Our back end is now complete with APIs to upload and download photos. The mobile app can take a photo, check it for happy Xamarin developers and upload the photo to the back end.

The next step is to extend `IAzureService` to be able to download photos to the mobile device. Head back to your Xamarin mobile app, where you will need to add code to the Azure service to download all the photo metadata as well as downloading the individual photos.

## 1. Installing Xamarin.Essentials NuGet Package

Each platform has different rules around file storage, and different locations where files are stored. Handling the different cases for iOS, Android and UWP is complicated and involves some platform specific code (similar to how we implemented the authentication).

To make life easier for Xamarin developers, Microsoft created a NuGet package called [Xamarin.Essentials](https://www.nuget.org/packages/Xamarin.Essentials) that provides cross-platform implementations of common platform-specific functionality including a helper to provide a storage location that works on all platforms.

[Xamarin.Essentials.FileSystem](https://docs.microsoft.com/xamarin/essentials/file-system-helpers/?WT.mc_id=mobileappsoftomorrow-workshop-jabenn) class has two directories available; `CacheDirectory` is a local cache for transient data; and `AppDataDirectory` for application data that should be backed.

1. Open the Xamarin.Forms app in Visual Studio

2. (PC) In Visual Studio, right-click the `HappyXamDevs` solution > **Manage NuGet Packages For Solution..**

    - (Mac) In Visual Studio for Mac, right-click the `HappyXamDevs` project > **Add** > **Add NuGet Packages**

3. (PC) In the **NuGet Package Manager** window, select **Browse**

    - (Mac) _Skip this step_

4. In the **NuGet Package Manager** window, in the search bar, enter **Xamarin.Essentials**

5. In the **NuGet Package Manager** window, in the search results, select **Xamarin.Essentials**

6. (PC) In the **NuGet Package Manager** window, select **Install**

    - (Mac) In the **NuGet Package Manager** window, select **Add Package**

7. (PC) _Skip this step_

    - (Mac) In Visual Studio for Mac, right-click the `HappyXamDevs.Android` project > **Add** > **Add NuGet Packages**

8. (PC) _Skip this step_

    - (Mac) In the **NuGet Package Manager** window, in the search results, select **Xamarin.Essentials**


9. (PC) _Skip this step_

    - (Mac) In the **NuGet Package Manager** window, select **Add Package**

10. (PC) _Skip this step_

    - (Mac) In Visual Studio for Mac, right-click the `HappyXamDevs.iOS` project > **Add** > **Add NuGet Packages**

11. (PC) _Skip this step_

    - (Mac) In the **NuGet Package Manager** window, in the search results, select **Xamarin.Essentials**

12. (PC) _Skip this step_

    - (Mac) In the **NuGet Package Manager** window, select **Add Package**

13. In the Visual Studio Solution Explorer, open **HappyXamDevs.Android** > **MainActivity.cs**

14. In the **MainActivity.cs** editor, enter the following code:

```csharp
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;

namespace HappyXamDevs.Droid
{
    [Activity(Label = "HappyXamDevs", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, bundle);
            Xamarin.Essentials.Platform.Init(this, bundle);
            LoadApplication(new App());
        }
    }
}
```
**About the Code**
>
> `Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);` allows `Xamarin.Essentials` to handle Android permissions requests
>
> `Xamarin.Essentials.Platform.Init(this, bundle);` ensures the `Xamarin.Essentials` libraries are initialized on startup

## 2. Creating a data object for photo metadata

The Azure Function, `GetAllPhotosMetadata`, returns a collection of JSON objects with a load of fields; some that we want, some that we don't.

We will  create a simple C# data object in our mobile app that deserializes these JSON objects into the relevant fields.

| Property    | Type       | Description                                                        |
| ----------- | ---------- | ------------------------------------------------------------------ |
| `Name`      | `string`   | The name of the Blob that contains the actual photo                |
| `Caption`   | `string`   | The caption for the photo generated by the Computer Vision service |
| `Tags`      | `string[]` | The tags for the photo generated by the Computer Vision service    |
| `Timestamp` | `long`     | A timestamp for when the document was created                      |

1. In the Visual Studio Solution Explorer, right-click on the `HappyXamDevs` project > **Add** > **New Folder**

2. In the Visual Studio Solution Explorer, name the new folder `Models`

3. In the Visual Studio Solution Explorer, right-click on the newly created **Models** folder > **Add** > **Class**

    - (Mac) On Visual Studio for Mac, right-click on the newly created **Models** folder > **Add** > **New File**

4. In the **Add New Item** window, name the file `PhotoMetadataModel.cs`

5. In the **PhotoMetadataModel.cs** editor, enter the following code:

```csharp
using System.IO;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace HappyXamDevs.Models
{
    public class PhotoMetadataModel
    {
        public string FileName => Path.Combine(FileSystem.CacheDirectory, $"{Name}.jpg");

        public string Caption { get; set; }

        public string Name { get; set; }

        public string[] Tags { get; set; }

        [JsonProperty("_ts")]
        public long Timestamp { get; set; }
    }
}
```

> **Note:** Every Cosmos DB document gets a timestamp field created automatically called `_ts`, and this contains the time the document was inserted or last updated using a UNIX timestamp (the number of elapsed seconds since January 1, 1970). We can tell `Newtonsoft.Json` to map the `_ts` field to the `Timestamp` property using the `JsonProperty` attribute on the property.

> **Note:** For a production quality app we should also think about how the app will work without an internet connection by caching the metadata and reducing network load. We should also consider connectivity and try not to download if the device is offline, something you can check using [Xamarin.Essentials.Connectivity](https://docs.microsoft.com/xamarin/essentials/connectivity/?WT.mc_id=mobileappsoftomorrow-workshop-jabenn). 

## 3. Downloading individual photos

For each photo metadata item that will be downloaded, our app will download the actual photos. This is a slow network call, so ideally the app should cache these photos locally to avoid having to re-download the files each time. The method to do this should check if the file exists using the Blob name as the file name, and if it doesn't exist, download the image.

1. In the Visual Studio Solution Explorer, open **HappyXamDevs** > **Services** > **IAzureService.cs**

2. In the **IAzureService.cs** editor, enter the following code:

```csharp
using System.Collections.Generic;
using System.Threading.Tasks;
using HappyXamDevs.Models;
using Plugin.Media.Abstractions;

namespace HappyXamDevs.Services
{
    public interface IAzureService
    {
        Task<bool> Authenticate();

        Task DownloadPhoto(PhotoMetadataModel photoMetadata);

        Task<IEnumerable<PhotoMetadataModel>> GetAllPhotoMetadata();

        bool IsLoggedIn();

        Task UploadPhoto(MediaFile photo);

        Task<bool> VerifyHappyFace(MediaFile photo);
    }
}
```

3. In the Visual Studio Solution Explorer, open **HappyXamDevs** > **Services** > **AzureServiceBase.cs**

4. In the **AzureServiceBase.cs** editor, enter the following using statements:

```csharp
using System.IO;
using System.Net.Http;
using Xamarin.Essentials;
using HappyXamDevs.Models;
```

5. In the **AzureServiceBase.cs** editor, enter the following method:

```csharp
public async Task DownloadPhoto(PhotoMetadataModel photoMetadata)
{
    if (File.Exists(photoMetadata.FileName))
        return;

    var result = await Client.InvokeApiAsync($"photo/{photoMetadata.Name}", HttpMethod.Get, new Dictionary<string, string>());

    var photo = result["photo"].Value<string>();
    var bytes = Convert.FromBase64String(photo);

    using (var fs = new FileStream(photoMetadata.FileName, FileMode.CreateNew))
        await fs.WriteAsync(bytes, 0, bytes.Length);
}
```

> **About the Code**
>
> `if (File.Exists(photoMetadata.FileName))` first checks to see if a file exits; if it does we won't try to re-downlaod it
>
> `await Client.InvokeApiAsync($"photo/{photoMetadata.Name}", HttpMethod.Get, new Dictionary<string, string>());` downloads the photo from the Azure Function `GetPhoto`
>
> `var photo = result["Photo"].Value<string>();` retrieves the photo as a Base64 string from the JSON response
>
> `var bytes = Convert.FromBase64String(photo);` converts the Base64 string into a `Byte[]` so that it can be saved to the file system
>
> `await fs.WriteAsync(bytes, 0, bytes.Length);` saves the photo to the file system on our mobile device

## 4. Downloading the photo metadata

1. In the **AzureServiceBase.cs** editor, add the following method:

```csharp
public async Task<IEnumerable<PhotoMetadataModel>> GetAllPhotoMetadata()
{
    var allMetadata = await Client.InvokeApiAsync<List<PhotoMetadataModel>>(PhotoResource, HttpMethod.Get, new Dictionary<string, string>());

    foreach (var metadata in allMetadata)
        await DownloadPhoto(metadata);

    return allMetadata;
}
```

> **About the Code**
>
> `await Client.InvokeApiAsync<List<PhotoMetadataModel>>(PhotoResource, HttpMethod.Get, new Dictionary<string, string>());` retrieves the photo metada from the Azure Function, `GetAllPhotoMetata`

## Next step

Now that you have photos downloaded, the next step is to [show these photos on the UI](./12-ShowPhotosOnMobileApp.md).
