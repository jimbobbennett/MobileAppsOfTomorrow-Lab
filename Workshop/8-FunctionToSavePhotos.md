# Create an Azure Function to Save Photos to Azure Blob Storage

Now that we have Azure Blob Storage configured, we need a way to put images into this storage.

Although it is possible to make your blob storage publicly writable from your app, this is a security hole. It is better to use a function to write to the blob storage instead, as this function can be secured behind the Facebook authentication you set up earlier in this workshop.

In this section you will be creating and configuring Azure Functions from inside the portal as this is the quickest way to get going. In a real world app you would develop these functions using [Visual Studio](https://docs.microsoft.com/azure/azure-functions/functions-develop-vs/?WT.mc_id=mobileappsoftomorrow-workshop-jabenn), or [Visual Studio Code](https://code.visualstudio.com/?WT.mc_id=mobileappsoftomorrow-workshop-jabenn) using the [Azure Functions Extension](https://marketplace.visualstudio.com/items?itemName=ms-azuretools.vscode-azurefunctions&WT.mc_id=mobileappsoftomorrow-workshop-jabenn).

## 1. Creating your first Azure Function

Azure Functions are started by [triggers](https://docs.microsoft.com/azure/azure-functions/functions-triggers-bindings/?WT.mc_id=mobileappsoftomorrow-workshop-jabenn). These include timer triggers (scheduler), HTTP triggers that are invoked when a request is made to a web endpoint, Blog triggers and more.

HTTP triggers are an incredibly useful way of building a REST service, and you can use one of these to create a function endpoint that your app can call, passing the photo. REST APIs usually have named endpoints (URLs) that you can query using HTTP methods such as GET or POST. Ideally you API should follow this standard. The API you will be building will be:

`https://<YourFunctionApp>.azurewebsites.net/api/photo`

By the end of this workshop we will be able to send a request using the GET method to this API to get the entire collection of photos. You will also be able to send a GET to `/api/photo/{name}` to get an individual photo by name, and to POST a photo to `/api/photo`.

In this step we will be implementing the POST method.

### 1a. Creating the Azure Function

1. In your browser, navigate to the [Azure portal](https://portal.azure.com/?WT.mc_id=mobileappsoftomorrow-workshop-jabenn)
2. In the Azure Portal, navigate to your Function App, **HappyXamDevsFunction-[Your Last Name]**
3. On the left-hand menu, click **Functions**
4. At the top of the **Functions** page, click the **+ New Function**
5. On the **Choose a template...** page, click **HTTP Trigger**
6. On the **New Function** slide-out menu, make the following selections:
    - **Name**: UploadPhoto
    - **Authorization level**: Anonymous
7. On the **New Function** slide-out menu, click **Create**

### 1b. Configuring the Azure function

By default, HTTP triggers support GET and POST methods. This function will only need to support POST. It will also be configured using the `photo` route template.

1. On the left-hand menu, expand the **UploadPhoto** drop-down
2. On the left-hand menu, under **UploadPhoto**, select the **Integrate**
3. In the **Integrate** window, under **Triggers**, select **HTTP (req)**
4. At the bottom of the page, in the **HTTP trigger** frame, make the following selections:
    - **Allowed Http methods:** Selected methods
    - **Request parameter name:** req
    - **Route Template**: photo
    - **Authorization level:** Anonymous
    - **Selected HTTP methods**: Post

    > Note: Uncheck all other **Selected HTTP methods**

5. In the **HTTP trigger** window, click **Save**
6. In the **Integrate** dashboard, under **Outputs**, click **+ New Output**
7. In the **New Output** window, select **Azure Queue Storage**
8. In the **New Output** window, select **Select**
9. In the **Azure Queue Storage output** window, if prompted to install an extension, click **Install**
10. In the **Azure Queue Storage output** window, stand by until the extension installation completes
11. In the **Azure Queue Storage output** window, enter the following:
    - **Message parameter name:** blobNameCollector
    - **Queue name:** processblobqueue
    - **Storage account connection:** AzureWebJobStorage
12. In the **Azure Queue Storage output** dashboard, click **Save**

### 1c. Writing the code

The mobile app will need to send the photo to this function. One easy way to do it is to Base64 encode the binary image data (so that it becomes a `string`), then send this as part of a JSON payload. This method allows you in the future to extend the data being set by adding more fields.

The JSON document you will send will be a list of name/value pairs and will have the following format:

```json
{
    "Photo" : "<Base64 encoded photo>"
}
```

You'll actually implement the sending of this data later in this part, but for now you can set up the function to be ready to receive it. The `"photo"` value will be retrieved from the HTTP request, converted back to binary data and saved into Blob storage. To access Blob storage, you will need to add a NuGet package to the function.

1. On the left-hand side of the Azure Functions Dashboard, select **UploadPhoto**
2. In the **UploadPhoto** window, scroll to right-to-left until the right-hand menu is visible
3. On the right-hand menu, select **View Files**
4. In the **View Files** window, click the **+ Add**
5. In the **file name** entry, enter `function.proj`
6. Press the **Return** key on the keyboard to save the new file
7. In the **function.proj** text editor, enter the following:

```xml
<Project Sdk="Microsoft.NET.Sdk">
    <PropertyGroup>
        <TargetFramework>netstandard2.0</TargetFramework>
    </PropertyGroup>

    <ItemGroup>
        <PackageReference Include="WindowsAzure.Storage" Version="9.3.3" />
    </ItemGroup>
</Project>
```

8. In the **function.proj** text editor, click **Save**

9. In the **View Files** pane on the right-hand menu, select **run.csx**

    > **Note**: This is a C# script file that contains the code that the function will run. The default new function contains a basic "Hello World" style function, which we will overwrite.

10. In the **run.csx** editor, enter the following code:

```csharp
#r "Microsoft.WindowsAzure.Storage"

using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;

public static async Task<IActionResult> Run(HttpRequestMessage req, IAsyncCollector<string> blobNameCollector, ILogger log)
{
    dynamic data = await req.Content.ReadAsAsync<object>();
    string photo = data?.Photo;
    var imageBytes = Convert.FromBase64String(photo);

    log.LogInformation($"Image Parsed");

    var connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");

    CloudStorageAccount.TryParse(connectionString, out var storageAccount);

    var blobClient = storageAccount.CreateCloudBlobClient();
    var blobContainer = blobClient.GetContainerReference("photos");

    var blobName = $"{Guid.NewGuid().ToString()}.jpeg";
    var blockBlob = blobContainer.GetBlockBlobReference(blobName);
    blockBlob.Properties.ContentType = "image/jpeg";

    await blockBlob.UploadFromByteArrayAsync(imageBytes, 0, imageBytes.Length);

    log.LogInformation($"Blob {blobName} created");

    await blobNameCollector.AddAsync(blobName);

    log.LogInformation($"Blob Name Added to Queue");

    return new CreatedResult(blockBlob.Uri, blockBlob);
}
```

> **About the Code**
>
> `dynamic data = await req.Content.ReadAsAsync<object>();` reads the HTTP request body into a dynamic variable, and extracts the `Photo` field as a `string`
>
> `var imageBytes = Convert.FromBase64String(photo);` converts `string photo` to `byte[] imageBytes` using the static `Convert.FromBase64String` method
>
> `var connectionString = System.Configuration.Environment.GetEnvironmentVariable("AzureWebJobs");` creates a connection to our Azure Blob Storage resource
>
> `var blobContainer = blobClient.GetContainerReference("photos");` gets access to the `photos` container via a Blob Client
>
> `var blockBlob = blobContainer.GetBlockBlobReference(blobName);` creates a new BlockBlob inside the `photos` container
>
> `await blockBlob.UploadFromByteArrayAsync(imageBytes, 0, imageBytes.Length);`  uploads the photo to the `photos` container in Azure Blob Storage
>
> `return new CreatedResult(blockBlob.Uri, blockBlob);` returns an HTTP result status of **201 - Created**

11. In the **run.csx** editor, click **Save**

## 2. Calling the Azure Function from the Mobile App

When you create an Azure Function, the API end point for the function defaults to `https://<FunctionsAppName>.azurewebsites.net/api/<FunctionName>` or `https://<FunctionsAppName>.azurewebsites.net/api/{Route template}`

- **E.g.** `https://HappyXamDevsFunction-Minnick.azurewebsites.net/api/UploadPhoto` or `https://HappyXamDevsFunction-Minnick.azurewebsites.net/api/photo`

To call this function we will send a POST request to this end point passing a JSON payload with the photo encoded as a Base64 string, and because it is behind authentication you would also need to pass an authentication token as an HTTP header.

To make it easier to use Azure Functions, the `MobileClient` class we used earlier to authenticate provides a way to call an API - essentially any function in the `api` path, and it will automatically pass the required authentication headers for you. All you have to do is call a method on the mobile client, passing the HTTP method you want to use, the function name and the payload.

### 2a. Adding `UploadPhoto(MediaFile photo)` to BaseAzureService.cs

1. In the Visual Studio Solution explorer, open **HappyXamDevs** > **Services** > **IAzureService.cs**

2. In the **IAzureService.cs** editor, enter the following code:

```csharp
using System.Threading.Tasks;
using Plugin.Media.Abstractions;

namespace HappyXamDevs.Services
{
    public interface IAzureService
    {
        bool IsLoggedIn();
        Task<bool> Authenticate();
        Task<bool> VerifyHappyFace(MediaFile photo);
        Task UploadPhoto(MediaFile photo);
    }
}
```

3. In the Visual Studio Solution explorer, open **HappyXamDevs** > **Services** > **AzureServiceBase.cs**

4. In the **AzureServiceBase.cs** editor, add the following `using` statement:

```csharp
using Newtonsoft.Json.Linq;
```

5. In the **AzureServiceBase.cs** editor, add the following constant field:

```csharp
private const string PhotoResource = "photo";
```

6. In the **AzureServiceBase.cs** editor, add the following method:

```csharp
public async Task UploadPhoto(MediaFile photo)
{
    using (var photoStream = photo.GetStream())
    {
        var bytes = new byte[photoStream.Length];
        await photoStream.ReadAsync(bytes, 0, Convert.ToInt32(photoStream.Length));

        var content = new
        {
            Photo = Convert.ToBase64String(bytes)
        };

        var json = JToken.FromObject(content);

        await Client.InvokeApiAsync(PhotoResource, json);
    }
}
```

> **About the Code**
>
> `await photoStream.ReadAsync(bytes, 0, Convert.ToInt32(photoStream.Length));` extracts image into a `byte[]`
>
> `var content = new { Photo = Convert.ToBase64String(bytes) };` creates an anonymous object containing the property `string Photo` which contains our image as a Base64 string
>
> `var json = JToken.FromObject(content);` serializes `content` into JSON
>
> `await Client.InvokeApiAsync(PhotoResource, json);` makes an authenticated call to our Azure Functions and uploads our photo

### 2b. Use `UploadPhoto` in `MainViewModel`

1. In the Visual Studio Solution Explorer, open **HappyXamDevs** > **ViewModels** > **MainViewModel.cs**

2. In the **MainViewModel.cs** editor, update `SelectFromLibrary()`

```csharp
private async Task SelectFromLibrary()
{
    var options = new PickMediaOptions { PhotoSize = PhotoSize.Medium };
    var photo = await CrossMedia.Current.PickPhotoAsync(options);

    if (await ValidatePhoto(photo))
        await azureService.UploadPhoto(photo);
}
```

3. In the **MainViewModel.cs** editor, update `TakePhoto()`

```csharp
private async Task TakePhoto()
{
    var options = new StoreCameraMediaOptions
    {
        PhotoSize = PhotoSize.Medium,
        DefaultCamera = CameraDevice.Front
    };
    var photo = await CrossMedia.Current.TakePhotoAsync(options);

    if (await ValidatePhoto(photo))
        await azureService.UploadPhoto(photo);
}
```

## 3. Test Azure Storage

## 3a. Test Azure Storage, Android

1. In Visual Studio, right-click on **HappyXamDevs.Android** > **Set as Startup Project**

2. (PC) In Visual Studio, select **Debug** > **Start Debugging**
    - (Mac) In Visual Studio for Mac, select **Run** > **Start Debugging**

3. On the Android device, if the **LoginPage** complete the login flow

4. On the Android device, on the **MainPage**, tap the Camera icon

5. On the Android device, if prompted for permission, tap **Allow**

6. On the Android device, ensure the Camera appears

7. On the Android device, take a happy-looking selfie

8. In the [Azure portal](https://portal.azure.com/?WT.mc_id=mobileappsoftomorrow-workshop-jabenn), navigate to the Azure Blob Storage instance **happyxamdevs[Your Last Name]**
    - E.g. happyxamdevsminnick

9. In the **Storage Account** dashboard, on the left-hand menu, select **Blobs**

10. On the **Blobs** page, select **photos**

11. On the **Photos** page, ensure a blob has been added

![A blob saved in blob storage](../Images/PortalPhotoSavedAsBlob.png)

## 3b. Test Azure Storage, iOS

1. In Visual Studio, right-click on **HappyXamDevs.iOS** > **Set as Startup Project**

2. (PC) In Visual Studio, select **Debug** > **Start Debugging**
    - (Mac) In Visual Studio for Mac, select **Run** > **Start Debugging**

3. On the iOS device, if the **LoginPage** complete the login flow

4. On the iOS device, on the **MainPage**, tap the Camera icon

5. On the iOS device, if prompted for permission, tap **Allow**

6. On the iOS device, ensure the Camera appears

7. On the iOS device, take a happy-looking selfie

8. In the [Azure portal](https://portal.azure.com/?WT.mc_id=mobileappsoftomorrow-workshop-jabenn), navigate to the Azure Blob Storage instance **happyxamdevs[Your Last Name]**
    - E.g., happyxamdevsminnick

9. In the **Storage Account** dashboard, on the left-hand menu, select **Blobs**

10. On the **Blobs** page, select **photos**

11. On the **Photos** page, ensure a blob has been added

![A blob saved in blob storage](../Images/PortalPhotoSavedAsBlob.png)

## 3c. Test Azure Storage, UWP

1. In Visual Studio, right-click on **HappyXamDevs.UWP** > **Set as Startup Project**

2. (PC) In Visual Studio, select **Debug** > **Start Debugging**
    - (Mac) In Visual Studio for Mac, select **Run** > **Start Debugging**

3. On the UWP device, if the **LoginPage** complete the login flow

4. On the UWP device, on the **MainPage**, tap the Camera icon

5. On the UWP device, if prompted for permission, tap **Allow**

6. On the UWP device, ensure the Camera appears

7. On the UWP device, take a happy-looking selfie

8. In the [Azure portal](https://portal.azure.com/?WT.mc_id=mobileappsoftomorrow-workshop-jabenn), navigate to the Azure Blob Storage instance **happyxamdevs[Your Last Name]**
    - E.g., happyxamdevsminnick

9. In the **Storage Account** dashboard, on the left-hand menu, select **Blobs**

10. On the **Blobs** page, select **photos**

11. On the **Photos** page, ensure a blob has been added

![A blob saved in blob storage](../Images/PortalPhotoSavedAsBlob.png)

## Next step

Now that you have a function to write photos to Blob storage that is called from your mobile app, the next step is to [create a blob storage trigger to analyze the photos and save the results to Cosmos DB](./9-BlobStorageTrigger.md).
