# Creating Azure Function to Load Photos

So far we have an app that can take a photo, check for happy faces then upload the photo to an Azure function that saves it in Blob Storage. A queue trigger is then fired that analyses the photo to get a description and tags, and stores this data in Cosmos DB.

The next step is to show a timeline on the mobile app of all the photos taken. To do this we will need two more APIs, one to retrieve the metadata for all the photos that have been taken, and one to download the Blobs for each photo.

> Having 2 APIs is more efficient as the mobile device can cache images locally and only request new ones that it needs. In a production app you would normally take this further and only return metadata for new photos using some kind of timestamp.

## 1. Creating a function to load metadata for all photos

The first Function to create will return the metadata for all photos using a Cosmos DB function binding - this time an input binding. We will bind a Cosmos DB collection to the input parameter of the Function, either returning the whole collection or just a query. For this Function, you will need the entire collection.

1. In the browser, navigate to the [Azure Portal](https://portal.azure.com/?WT.mc_id=mobileappsoftomorrow-workshop-jabenn)
2. In the Azure Portal, navigate to **HappyXamDevsFunction-[Your Last Name]**
    - E.g. HappyXamDevsFunction-Minnick

3. In the **Functions** dashboard, on the left-hand menu, click **Functions**
4. In the **Functions** window, click **+ New Function**
5. In the **Add new..** window, select **HTTP trigger**
6. In the **Http trigger** slide out, enter the following:
    - **Name:** GetAllPhotosMetadata
    - **Authorization level:** Anonymous
7. In the **Http trigger** slide out, click **Create**
8. In the **run.csx editor**, enter the following code:

```csharp
using Microsoft.AspNetCore.Mvc;

public static IActionResult Run(HttpRequestMessage req, IEnumerable<dynamic> documents, ILogger log)
{
    return new OkObjectResult(documents);
}
```
> **About the Code**
>
> `IEnumerable<dynamic> documents` contains all documents from CosmosDb
>
> `return new OkObjectResult(documents)` returns an **OK - 200** message containing the CosmosDb documents in the response body

9. In the **run.csx editor**, click **Save**

10. On the **Functions** dashboard, on the left-hand menu, select **GetAllPhotosMetadata** > **Integrate**
11. On the **Integrate** window, select **HTTP (req)**
12. On the **HTTP (req)** window, enter the following:
    - **Allowed Http methods:** Selected methods
    - **Request parameter name:** req
    - **Route Template**: photo
    - **Authorization level:** Anonymous
    - **Selected HTTP methods**: Get

    > Note: Uncheck all other **Selected HTTP methods**

13. On the **HTTP (req)** window, click **Save**
14. On the **Integrate** window, under **Inputs**, select **+ New Input**
15. In the **New Input** window, scroll to the bottom and select **Azure Cosmos DB**
16. In the **New Input** window, click **Select**
17. In the **Azure Cosmos DB input** window, enter the following:
    - **Document parameter name:** documents
    - **Database name:** Photos
    - **Collection name:** PhotoMetadata
    - **Azure Cosmos DB account connection** happyxamdevs-[Your Last Name]_DOCUMENTDB
    - **Document ID:** [Leave Blank]
    - **Partition Key:** [Leave Blank]
    - **SQL Query:** [Leave Blank]
18. In the **Azure Cosmos DB input** window, click **Save**

## 2. Creating a function to retrieve a photo

Next we will write a function that will take the name of a Blob and return that Blob, encoded as Base64 string in a JSON object.

This function will be routed to the `photo/{blobName}` REST resource, such that making an HTTP GET call to `https://<YourFunctionApp>.azurewebsites.net/api/photo/<photo blobName>`  will return that photo blob.

1. In the **Functions** dashboard, on the left-hand menu, click **Functions**
2. In the **Functions** window, click **+ New Function**
3. In the **Add new..** window, select **HTTP trigger**
4. In the **HTTP trigger** slide out, enter the following:
    - **Name:** GetPhoto
    - **Authorization Level:** Anonymous
5. On the **HTTP trigger** slide out, click **Create**
6. In the **run.csx** editor, enter the following code:

```csharp
#r "Microsoft.WindowsAzure.Storage"

using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage;

public static async Task<IActionResult> Run(HttpRequestMessage req, string blobName, ILogger log)
{
    var connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");

    CloudStorageAccount.TryParse(connectionString, out var storageAccount);
    var blobClient = storageAccount.CreateCloudBlobClient();

    var blobContainer = blobClient.GetContainerReference("photos");
    var photoBlob = blobContainer.GetBlockBlobReference(blobName);

    var filePath = $"D:\\home\\blobPhoto{System.DateTime.UtcNow.Ticks}.jpeg";
    using (var fileStream = System.IO.File.OpenWrite(filePath))
    {
        await photoBlob.DownloadToStreamAsync(fileStream);
    }

    using (var fileStream = System.IO.File.OpenRead(filePath))
    using (var memoryStream = new MemoryStream())
    {
        fileStream.CopyTo(memoryStream);

        var resultObject = new
        {
            Photo = memoryStream.ToArray()
        };

        return new OkObjectResult(resultObject);
    }
}
```

> **About the Code**
>
> `string blobName` is the parameter name sent from the mobile app
>
> `await photoBlob.DownloadToStreamAsync(fileStream);` downloads the blob to a file
>
> `memoryStream.ToArray()` converts the `Stream` to a Base64 array
>
> `return new OkObjectResult(resultObject);` returns an **OK - 200** response containing the photoBlob

7. In the **run.csx** editor, click **Save**
8. In the **Functions** page, scroll to right-to-left until the right-hand menu is visible
9. On the right-hand menu, select **View Files** 
10. In the **View Files** window, click the **+ Add**
11. In the **file name** entry, enter `function.proj`
12. Press the **Return** key on the keyboard to save the new file
13. In the **function.proj** text editor, enter the following:

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

14. In the **function.proj** editor, click **Save**
15. On the **Functions** page, on the left-hand menu, select **GetPhoto** > **Integrate**
16. On the **Integrate** window, select **Http (req)**
17. In the **Http trigger** window, enter the following:
    - **Allowed Http methods:** Selected methods
    - **Request parameter name:** req
    - **Route Template**: photo/{blobName}
    - **Authorization level:** Anonymous
    - **Selected HTTP methods**: Get

    > Note: Uncheck all other **Selected HTTP methods**

18. In the **Http trigger** window, click **Save**


## Next step

Now that you have functions to download photo metadata and individual photos, the next step is to [use these in your Xamarin app to download photos](./10-DownloadPhotosToMobileApp.md).
