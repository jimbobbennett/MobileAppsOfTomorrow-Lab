# Create an Azure Function to save photos to blob storage

Now that we have Azure Blob Storage configured, we need a way to put images into this storage.

Although it is possible to make your blob storage publicly writable from your app, this is a security hole. It is better to use a function to write to the blob storage instead, as this function can be secured behind the Facebook authentication you set up earlier in this workshop.

In this section you will be creating and configuring Azure Functions from inside the portal as this is the quickest way to get going. In a real world app you would develop these functions using [Visual Studio](https://docs.microsoft.com/azure/azure-functions/functions-develop-vs/?WT.mc_id=mobileappsoftomorrow-workshop-jabenn), or [Visual Studio Code](https://code.visualstudio.com/?WT.mc_id=mobileappsoftomorrow-workshop-jabenn) using the [Azure Functions Extension](https://marketplace.visualstudio.com/items?itemName=ms-azuretools.vscode-azurefunctions&WT.mc_id=mobileappsoftomorrow-workshop-jabenn).

## Creating your first Azure Function

Azure Functions are started by [triggers](https://docs.microsoft.com/azure/azure-functions/functions-triggers-bindings/?WT.mc_id=mobileappsoftomorrow-workshop-jabenn). These include timer triggers (scheduler), HTTP triggers that are invoked when a request is made to a web endpoint, Blog triggers and more.

HTTP triggers are an incredibly useful way of building a REST service, and you can use one of these to create a function endpoint that your app can call, passing the photo. REST APIs usually have named endpoints (URLs) that you can query using HTTP methods such as GET or POST. Ideally you API should follow this standard. The API you will be building will be:

`https://<YourFunctionApp>.azurewebsites.net/api/photo`

By the end of this workshop you will be able to send a request using the GET method to this API to get the entire collection of photos. You will also be able to send a GET to `/api/photo/{name}` to get an individual photo by name, and to POST a photo to `/api/photo`.

In this step you will be implementing the POST method.

Azure Functions are named functions, so you would name them based off a naming convention that makes sense to you. You can then set up a [_Route Template_](https://docs.microsoft.com/en-gb/azure/azure-functions/functions-bindings-http-webhook?WT.mc_id=mobileappsoftomorrow-workshop-jabenn#customize-the-http-endpoint) to assign a REST resource and HTTP method combination to a function. For example you can create a function called "GetPhotos" and assign this to all GET requests made to `/api/photo`.

### 1. Creating the Azure Function

1. In your browser, navigate to the [Azure portal](https://portal.azure.com/?WT.mc_id=mobileappsoftomorrow-workshop-jabenn)
2. In the Azure Portal, navigate to your Function App, **HappyXamDevsFunction-[Your Last Name]**
3. On the left-hand menu, click **Functions**
4. At the top of the **Functions** page, click the **+ New Function**
5. On the **Choose a template...** page, click **HTTP Trigger**
6. On the **New Function** slide-out menu, make the following selections:
    - **Name**: UploadPhoto
    - **Authorization level**: Anonymous
7. On the **New Function** slide-out menu, click **Create**

    ![Creating a new C# HTTP Triggered Azure Function](../Images/PortalnewHttpTrigger.png)

### 2. Configuring the Azure function

By default, HTTP triggers support GET and POST methods. This function will only need to support POST. It will also be configured using the `photo` resource.

1. On the left-hand menu, expand the **UploadPhoto** drop-down 
2. On the left-hand menu, under **UploadPhoto**, select the **Integrate** node
3. In the **Integrate** dashboard, make the following selections:
    - **Route Template**: photo
    - **Selected HTTP methods**: Post

    > Note: Uncheck all other **Selected HTTP methods**

4. In the **Integrate** dashboard, click **Save**

    ![Configuring the HTTP method and route for the UploadPhoto function](../Images/PortalUploadPhotoIntegrate.png)

### 3. Add Connection String to Function App Settings

To connect to blob storage, the function needs a connection string. Rather than hard-coding this, it can be configured as an application setting.

1. In the [Azure portal](https://portal.azure.com/?WT.mc_id=mobileappsoftomorrow-workshop-jabenn), navigate to the Blob Storage, **happyxamdevsstorage**
2. On the left-hand menu of the Blob Storage dashboard, select **Access Keys**
3. In the **Access Keys** window, copy **key1** **Connection String**
4. In the [Azure portal](https://portal.azure.com/?WT.mc_id=mobileappsoftomorrow-workshop-jabenn), navigate to the Functions App, **HappyXamDevsFunction-[LastName]**
5. On the **Functions** down-down, select **UploadPhoto**
6. On the **Overview** tab, click **Application Settings**

    ![Clicking the Application settings in the Function app](../Images/PortalSelectFunctionAppSettings.png)

5. In the **Application Settings** tab, scroll down to the section marked **Application settings**  

6. In the **Application Settings** section, select **+ Add new setting**

7. For the new setting, set the following values:
    - **App Setting Name**: BlobStorageConnectionString
    - **Value**: [Your Blob Storage Connection String]

8. In the **Application Settings** tab, scroll to the top of the page

9. In the **Application Settings** tab, select **Save**

    ![Adding the blob storage connection string as an application setting](../Images/PortalAddingBlobConnectionSetting.png)

### Writing the code

The mobile app will need to send the photo to this function. One easy way to do it is to Base64 encode the binary image data (so that it becomes a `string`), then send this as part of a JSON payload. This method allows you in the future to extend the data being set by adding more fields.

The JSON document you will send will be a list of name/value pairs and will have the following format:

```json
{
    "Photo" : "<Base64 encoded photo>"
}
```

You'll actually implement the sending of this data later in this part, but for now you can set up the function to be ready to receive it. The `"photo"` value will be retrieved from the HTTP request, converted back to binary data and saved into Blob storage. To access Blob storage, you will need to add a NuGet package to the function.

1. On the left-hand side of the Azure Functions Dashboard, select **UploadPhoto** 
2. In the **UploadPhoto**, scroll to right-to-left until the right-hand menu is visible
3. On the right-hand menu, select **View Files** 
4. In the **View Files** window, click the **+ Add**
5. In the **file name** entry, enter `project.json`
6. Press the **Return** key on the keyboard to save the new file

    ![Adding a new file to the Azure Function](../Images/PortalAddFileToFunction.png)

7. In the **project.json** text editor, This file will open in the editor, enter the following:

```json
{
    "frameworks": {
        "net46":{
            "dependencies": {
                "WindowsAzure.Storage": "9.3.3"
            }
        }
    }
}
```

8. In the **project.json** text editor, click **Save**

9. In the **View Files** pane on the right-hand menu, select **run.csx**

    > **Note**: This is a C# script file that contains the code that the function will run. The default new function contains a basic "Hello World" style function, which we will overwrite

10. In the **run.csx** editor, clear (delete) the existing Hello World code

11. In the **run.csx** editor, enter the following code to read the HTTP request body into a dynamic variable, and extracts the `Photo` field as a `string`:

    ```csharp
    dynamic data = await req.Content.ReadAsAsync<object>();
    string photo = data?.Photo;
    ```

    > **Note:** It is important to specify the type of the `photo` variable instead of using `var` otherwise a dynamic type is used. The explicit `string` variable type ensures the `Photo` property is read as a string.

    > **Note:** When data is sent in a JSON document using name/value pairs, we can extract the values by reading the content as a dynamic object, taking advantage of [C#'s `dynamic` keyword](https://docs.microsoft.com/dotnet/csharp/programming-guide/types/using-type-dynamic/?WT.mc_id=mobileappsoftomorrow-workshop-jabenn). Once a JSON payload is converted to a dynamic object, we can access the items in it as if they were properties of an object.

12. In the **run.csx** editor, enter the following code to convert `string photo` to `byte[] imageBytes` using the static `Convert.FromBase64String` method:

    ```csharp
    var imageBytes = Convert.FromBase64String(photo);
    ```

13. In the **run.csx** editor, enter the following code to create a connection to our Azure Blob Storage resource using the connection string

    ```csharp
    var connectionString = System.Configuration.Environment.GetEnvironmentVariable("BlobStorageConnectionString");
    ```

    > **Note:** `System.Configuration.Environment.GetEnvironmentVariable("BlobStorageConnectionString")` searches the Azure Function App's Application Settings for the **App Setting Name** `BlobStorageConnectionString` and returns returns its value

14. In the **run.csx** editor, enter the following code to create a `CloudStorageAccount` object connected to our Azure Blob Storage resource:

    ```csharp
    Microsoft.WindowsAzure.Storage.CloudStorageAccount.TryParse(connectionString, out Microsoft.WindowsAzure.Storage.CloudStorageAccount storageAccount);
    ```

15. In the **run.csx** editor, enter the following code to get access to the `photos` container by creating a Blob client and accessing its container:

    ```csharp
    var blobClient = storageAccount.CreateCloudBlobClient();
    var blobContainer = blobClient.GetContainerReference("photos");
    ```

16. In the **run.csx** editor, enter the following code to create a new BlockBlob inside the `photos` container and set its `ContentType` to `Jpeg`:

    ```csharp
    var blobName = Guid.NewGuid().ToString();
    var blockBlob = blobContainer.GetBlockBlobReference(blobName);
    blob.Properties.ContentType = "Jpeg";
    ```

    > **Note:** The BlockBlob requires a unique name for which we generate a new Guid

17. In the **run.csx** editor, enter the following code to upload `byte[] imageBytes` to the `photos` container in Azure Blob Storage:

    ```csharp
    await blockBlob.UploadFromByteArrayAsync(imageBytes, 0, imageBytes.Length);
    ```

18. In the **run.csx** editor, enter the following code to return an HTTP result status of **201 - Created**:

    ```csharp
    return new CreatedResult(blockBlob.Uri, blockBlob);
    ```

    > **Note:** Because the Function app uses authentication, you won't be able to test this function directly using the **Run** option inside the portal, unless you turn authentication off at the Function app level

19. In the **run.csx** editor, click **Save**

The full code of this function is below.

```csharp
using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;

public static async Task<IActionResult> Run(HttpRequestMessage req, ILogger log)
{
    dynamic data = await req.Content.ReadAsAsync<object>();
    string photo = data?.Photo;
    var imageBytes = Convert.FromBase64String(photo);

    log.LogInformation($"Image Parsed");

    var connectionString = Environment.GetEnvironmentVariable("BlobStorageConnectionString");

    CloudStorageAccount.TryParse(connectionString, out var storageAccount);

    var blobClient = storageAccount.CreateCloudBlobClient();
    var blobContainer = blobClient.GetContainerReference("photos");

    var blobName = Guid.NewGuid().ToString();
    var blockBlob = blobContainer.GetBlockBlobReference(blobName);
    blockBlob.Properties.ContentType = "Jpeg";

    await blockBlob.UploadFromByteArrayAsync(imageBytes, 0, imageBytes.Length);

    log.LogInformation($"Blob {blobName} created");

    return new CreatedResult(blockBlob.Uri, blockBlob);
}
```

## Calling the function from the mobile app

When you create an Azure Function, the API end point for the function defaults to `https://<FunctionsAppName>.azurewebsites.net/api/<FunctionName>` or `https://<FunctionsAppName>.azurewebsites.net/api/{Route template}`

    > **E.g.** `https://HappyXamDevsFunction-Minnick.azurewebsites.net/api/UploadPhoto` or `https://HappyXamDevsFunction-Minnick.azurewebsites.net/api/photo`

To call this function we will send a POST request to this end point passing a JSON payload with the photo encoded as a Base64 string, and because it is behind authentication you would also need to pass an authentication token as an HTTP header.

To make it easier to use Azure Functions, the `MobileClient` class you used earlier to authenticate provides a way to call an API - essentially any function in the `api` path, and it will automatically pass the required authentication headers for you. All you have to do is call a method on the mobile client, passing the HTTP method you want to use, the function name and the payload.

### Adding a method to the Azure Service to upload a photo

1. Head back to the Xamarin app and open the `IAzureService` interface. Add a new method to this interface to upload a photo.

    ```csharp
    Task UploadPhoto(MediaFile photo);
    ```

2. Open the `AzureServiceBase` implementation of this interface and add a constant to this class for the name of the photo REST resource.

    ```csharp
    const string PhotoResource = "photo";
    ```

3. Add a skeleton for the new `UploadPhoto` method.

    ```csharp
    public async Task UploadPhoto(MediaFile photo)
    {
    }
    ```

4. In this method, get the image from the `MediaFile` as a stream, and extract the image as bytes.

    ```csharp
    using (var s = photo.GetStream())
    {
        var bytes = new byte[s.Length];
        await s.ReadAsync(bytes, 0, Convert.ToInt32(s.Length));
    }
    ```

5. To create the JSON payload, you can create an [anonymous type](https://docs.microsoft.com/dotnet/csharp/programming-guide/classes-and-structs/anonymous-types/?WT.mc_id=mobileappsoftomorrow-workshop-jabenn) with a field called `photo`.

    ```csharp
    var content = new
    {
        Photo = Convert.ToBase64String(bytes)
    };
    ```

6. Next you need to serialize this to JSON. The best tool to handle JSON is the [Newtonsoft.Json](https://www.nuget.org/packages/Newtonsoft.Json/) NuGet package, and this is already installed for you as a dependency of the Azure mobile client package. You will need to add a using directive for the `Newtonsoft.Json.Linq` namespace.

    ```csharp
    var json = JToken.FromObject(content);
    ```

7. Finally, use the mobile client to make an authenticated call to the Azure Function, passing the JSON payload. The `InvokeApiAsync` method is used to invoke methods in the `/api/` path, so only needs the resource name.

    ```csharp
    await Client.InvokeApiAsync(PhotoResource, json);
    ```

    > By default when you use this overload that calls a function and passes data, the HTTP call is always a POST so there is no need to specify the method. If you did want to use a different method (for example a PUT to update a resource), you can add this as an additional parameter.

The full code for this method is shown below.

```csharp
public async Task UploadPhoto(MediaFile photo)
{
    using (var s = photo.GetStream())
    {
        var bytes = new byte[s.Length];
        await s.ReadAsync(bytes, 0, Convert.ToInt32(s.Length));

        var content = new
        {
            Photo = Convert.ToBase64String(bytes)
        };

        var json = JToken.FromObject(content);
        await Client.InvokeApiAsync(PhotoResource, json);
    }
}
```

### Using this method to upload a photo

1. Open the `MainViewModel`. Add a call to the new `UploadPhoto` method to the end of both the `TakePhoto` and `SelectFromLibrary` methods, after the validation check.

    ```csharp
    await azureService.UploadPhoto(photo);
    ```

## Test it out

Run the app on your platform of choice and take a photograph or select one from your library (ensuring of course it has happy Xamarin developers in it). You can see the function being called by opening the function in the Azure Portal and expanding the _Log_ tab before the Azure Function is called - this will show a live log of calls being made.

> If you want more details on what the function is doing you can add log statements. Every function is passed a `TraceWriter log` parameter, and this `log` parameter has methods to log out `Debug`, `Info` or `Error` messages. If you want more detail on what your function is doing add some log statements and save the function, before running you app again.

Once a photo has been uploaded you can see it by selecting the Blob storage resource, selecting _Browse blobs_, and selecting the _photos_ collection.

![A blob saved in blob storage](../Images/PortalPhotoSavedAsBlob.png)

> This app doesn't include any feedback whilst the photo is being uploaded. In a production quality app you should provide feedback during the upload and once it has completed (and show any errors if it fails, such as due to connection issues).

## Next step

Now that you have a function to write photos to Blob storage that is called from your mobile app, the next step is to [create a blob storage trigger to analyze the photos and save the results to Cosmos DB](./9-BlobStorageTrigger.md).