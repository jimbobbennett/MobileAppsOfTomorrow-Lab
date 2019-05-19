# Detect happy faces

For this app, only happy faces are allowed to be shared, so we will check that each photo contains happy faces.

We will use the power of artificial intelligence (AI), thanks to [Azure Cognitive Services FaceAPI](https://docs.microsoft.com/azure/cognitive-services/face/overview/?WT.mc_id=mobileappsoftomorrow-workshop-jabenn).

## 1. Configuring the Face API, Azure portal

1. In a browser, navigate to the [Azure Portal](https://portal.azure.com/?WT.mc_id=mobileappsoftomorrow-workshop-jabenn)

2. In the Azure Portal, navigate to the Resource Group **HappyXamDevs**

3. On the **HappyXamDevs** resource group dashboard, click **+ Add**

4. In the **Everything** page, in the search bar, enter **Face**

5. On the keyboard, press the **Enter** key

6. In the search results, select **Face**

    ![Searching for Face in the Azure portal](../Images/PortalSearchFace.png)

7. In the **Face** window, click **Create**

8. In the **Create** window, enter the following:

    - **Name:** HappyXamDevs-Face
    - **Subscription:** [Select your subscription]
    - **Location:** Pick the location closest to you, for example West Europe
    - **Pricing tier** F0
    - **Resource Group** HappyXamDevs

   > **Note:** **Pricing tier** "F0" is a free tier with a limited number of API calls; it is limited to 20 calls per minute, and 30,000 per month

9. In the Azure Portal, navigate to the Resource Group **HappyXamDevs**

10. On the **XamHappyDevs** Resource Group dashboard, select the newly created **HappyXamDevs-Face**

11. In the **XamHappyDevs-Face** dashboard, on the left-hand menu, select **Keys**

    ![Keys resource management](../Images/PortalFaceKeys.png)

12. On the **Keys** window, copy the value of `KEY 1`
    > **Note:** We will use this value in our mobile app

13. In the **XamHappyDevs-Face** dashboard, on the left-hand menu, select **Overview**

14. In the **Overview** window, copy the Base Url of **Endpoint**
    - Incorrect Base Url: `https://westus.api.cognitive.microsoft.com/face/v1.0`
    - Correct Base Url: `https://westus.api.cognitive.microsoft.com/`
    > **Note:** We will use this value in our mobile app

## 2. Adding Microsoft.Azure.CognitiveServices.Vision.Face NuGet Package

The Azure Cognitive Services FaceAPI is accessible from a NuGet package that provides wrappers around the available services. This can be used to detect faces in your photo.

### PC

1. In Visual Studio, right-click the `HappyXamDevs` solution > **Manage NuGet Packages For Solution..**

2. In the **NuGet Package Manager** window, select **Browse**

3. In the **NuGet Package Manager** window, check **Include prerelease**

4. In the **NuGet Package Manager** window, in the search bar, enter **Microsoft.Azure.CognitiveServices.Vision.Face**

5. In the **NuGet Package Manager** window, in the search results, select **Microsoft.Azure.CognitiveServices.Vision.Face**

6. In the **NuGet Package Manager** window, select **Install**

### Mac

1. In Visual Studio for Mac, right-click the `HappyXamDevs` project > **Add** > **Add NuGet Packages**
2. In the **NuGet Package Manager**, check **Show pre-release packages**

3. In the **NuGet Package Manager** window, in the search bar, enter **Microsoft.Azure.CognitiveServices.Vision.Face**

4. In the **NuGet Package Manager** window, in the search results, select **Microsoft.Azure.CognitiveServices.Vision.Face**

5. In the **NuGet Package Manager** window, select **Add Package**

6. You will see a licence agreement dialog, accept the licence to continue.

7. Repeat these steps for the `HappyXamDevs.Android` and  `HappyXamDevs.iOS` projects

## 3. Creating `VerifyHappyFace`

1. In the Visual Studio Solution Explorer, open **HappyXamDevs** > **Services** > **IAzureService.cs**

2. In the **IAzureService.cs** editor, enter the following code:

    ```csharp
    using System.Threading.Tasks;
    using Plugin.Media.Abstractions;

    namespace HappyXamDevs.Services
    {
        public interface IAzureService
        {
            Task<bool> VerifyHappyFace(MediaFile photo);
        }
    }
    ```

3. In the Visual Studio Solution Explorer, open **HappyXamDevs** > **Services** > **AzureService.cs**

4. In the **AzureService.cs** editor, add the following using statements:

    ```csharp
    using System.Linq;
    using System.Collections.Generic;
    using Microsoft.Azure.CognitiveServices.Vision.Face;
    using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
    using Plugin.Media.Abstractions;
    ```

5. In the **AzureService.cs** editor, add the following `readonly` field:

    ```csharp
    private readonly FaceClient faceApiClient = new FaceClient(new ApiKeyServiceClientCredentials("[YOUR API KEY HERE]"))
    {
        Endpoint = "[YOUR FACE API BASE URL]"
    };
    ```

    > **Note:** Replace `[Your API Key]` with the value from `KEY 1`
    >
    > **Note:** Replace `[Your Face API Base Url]` with the Base Url of the **Face API**
    > > **Warning:** Do not use the full URL; only use the base url

    - **Correct Base Url:** `https://westus.api.cognitive.microsoft.com/`
    - **Incorrect Base Url:** `https://westus.api.cognitive.microsoft.com/face/v1.0`

6. In the **AzureService.cs** editor, add the following method:

    ```csharp
    public async Task<bool> VerifyHappyFace(MediaFile photo)
    {
        using (var photoStream = photo.GetStream())
        {
            var faceAttributes = new List<FaceAttributeType> { FaceAttributeType.Emotion };

            var faces = await faceApiClient.Face.DetectWithStreamAsync(photoStream, returnFaceAttributes: faceAttributes);

            return faces.Any(f => f.FaceAttributes.Emotion.Happiness > 0.75);
        }
    }
    ```

    > ** About the Code**
    >
    > `using (var photoStream = photo.GetStream())` creates an object, `photoStream`, using the photo from the user
    >
    > `faceApi.Face.DetectWithStreamAsync` returns the emotion results from the Face API
    >
    > `faces.Any(f => f.FaceAttributes.Emotion.Happiness > 0.75)` searches the Face API results for `Happiness` value above 0.75; 0.75 is a confidence interval indicating that there is a 75% chance a happy face was found in the photo

## 4. Adding Face Detection to `MainViewModel.cs`

Now that you have a method on your Azure service to detect a happy face, you can call this in your main view model to validate that the photo contains happy faces. Open the `MainViewModel` class.

1. In the Visual Studio Solution Explorer, open **HappyXamDevs** > **ViewModels** > **MainViewModel.cs**

2. In the **MainViewModel.cs** editor, enter the following code:

    ```csharp
    using System.Threading.Tasks;
    using System.Windows.Input;
    using HappyXamDevs.Services;
    using Plugin.Media;
    using Plugin.Media.Abstractions;
    using Xamarin.Forms;

    namespace HappyXamDevs.ViewModels
    {
        public class MainViewModel : BaseViewModel
        {
            readonly IAzureService azureService;

            public MainViewModel()
            {
                TakePhotoCommand = new Command(async () => await TakePhoto());
                SelectFromLibraryCommand = new Command(async () => await SelectFromLibrary());
                azureService = DependencyService.Get<IAzureService>();
            }

            public ICommand SelectFromLibraryCommand { get; }
            public ICommand TakePhotoCommand { get; }

            private async Task SelectFromLibrary()
            {
                var options = new PickMediaOptions { PhotoSize = PhotoSize.Medium };
                var photo = await CrossMedia.Current.PickPhotoAsync(options);

                if (await ValidatePhoto(photo))
                    return;
            }

            private async Task TakePhoto()
            {
                var options = new StoreCameraMediaOptions
                {
                    PhotoSize = PhotoSize.Medium,
                    DefaultCamera = CameraDevice.Front
                };
                var photo = await CrossMedia.Current.TakePhotoAsync(options);

                if (await ValidatePhoto(photo))
                    return;
            }

            private async Task<bool> ValidatePhoto(MediaFile photo)
            {
                if (photo is null)
                    return false;

                var isHappy = await azureService.VerifyHappyFace(photo);

                if (isHappy)
                    return true;

                await Application.Current.MainPage.DisplayAlert("Sad panda",
                                                    "I can't find any happy Xamarin developers in this picture. Please try again.",
                                                    "Will do!");
                return false;
            }
        }
    }
    ```

    > **About the Code**
    >
    > `DependencyService.Get<IAzureService>();` retrieves the platform-specific implementation of `IAzureService`; i.e. If the app is running on an Android device, it will return `HappyXamDevs.Android.Services.AzureService`
    >
    > `ValidatePhoto` uses `IAzureService.VerifyHappyFace` to ensure a happy face is found in the photo

## 5. Test the Face API

### Android

1. In Visual Studio Solution Explorer, open **HappyXamDevs** > **ViewModels** > **MainViewModel.cs**

2. In Visual Studio, right-click on **HappyXamDevs.Android** > **Set as Startup Project**

3. - (PC) In Visual Studio, select **Debug** > **Start Debugging**
    - (Mac) In Visual Studio for Mac, select **Run** > **Start Debugging**

4. On the Android device, on the **MainPage**, tap the Camera icon

5. On the Android device, if prompted for permission, tap **Allow**

6. On the Android device, ensure the Camera appears

7. On the Android device, take a happy-looking selfie

8. You will not see an alert in the app

9. Repeat this with a sad looking selfie, and you will see an alert in the app.

### iOS

1. In Visual Studio Solution Explorer, open **HappyXamDevs** > **ViewModels** > **MainViewModel.cs**

2. In Visual Studio, right-click on **HappyXamDevs.iOS** > **Set as Startup Project**

3. - (PC) In Visual Studio, select **Debug** > **Start Debugging**
    - (Mac) In Visual Studio for Mac, select **Run** > **Start Debugging**

4. On the iOS device, on the **MainPage**, tap the Camera icon

5. On the iOS device, if prompted for permission, tap **Allow**

6. On the iOS device, ensure the Camera appears

7. On the iOS device, take a happy-looking selfie

8. You will not see an alert in the app

9. Repeat this with a sad looking selfie, and you will see an alert in the app.

### UWP

UWP only works on Windows, so if you are using Visual Studio for Mac, skip this step.

1. In Visual Studio Solution Explorer, open **HappyXamDevs** > **ViewModels** > **MainViewModel.cs**

2. In Visual Studio, right-click on **HappyXamDevs.UWP** > **Set as Startup Project**

3. In Visual Studio, select **Debug** > **Start Debugging**

4. On the UWP device, on the **MainPage**, tap the Camera icon

5. On the UWP device, if prompted for permission, tap **Allow**

6. On the UWP device, ensure the Camera appears

7. On the UWP device, take a happy-looking selfie

8. You will not see an alert in the app

9. Repeat this with a sad looking selfie, and you will see an alert in the app.

## Next step

Now that your app can take photos and verify that they are of happy developers, the next step is to [configure storage in Azure using Blob storage and CosmosDB](./6-ConfigureStorage.md) so that you have somewhere to upload your photos to.
