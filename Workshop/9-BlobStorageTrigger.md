# Creating an Azure Function with a Blob Storage Trigger

In the previous step we uploaded an image to Blob Storage using an Azure Function.

In this step we will create another Azure Function that uses a Blob Storage trigger making it run every time a new Blob is added or an existing Blob is updated.

The new Azure Function will use the [Azure Cognitive Services Computer Vision API](https://docs.microsoft.com/azure/cognitive-services/computer-vision/home/?WT.mc_id=mobileappsoftomorrow-workshop-jabenn) to generate an image description and some tags around what is in the photo, then upload its metadata to Cosmos DB.

## 1.Configuring Computer Vision API in the Azure portal

Before you can use the Computer Vision API, you will need an API key. You can get this by creating a resource in the Azure portal.

1. In the browser, navigate to the [Azure Portal](https://portal.azure.com/?WT.mc_id=mobileappsoftomorrow-workshop-jabenn)

2. On the left-hand toolbar, click **Create a resource**  

> **Note:** If the toolbar is collapsed, it will be shown as a green **+**

2. In the **New** dashboard, in the search bar, enter **Computer Vision**

3. On the keyboard, select the **Return** key

4. In th search results, select **Computer Vision**

    ![Searching for Computer Vision in the Azure portal](../Images/PortalSearchComputerVision.png)

5. In the **Computer Vision** window, click **Create**

6. In the **Create** window, enter the following:
    - **Name:** HappyXamDevs-ComputerVision
    - **Subscription:** [Your Azure Subscription]
    - **Location:** West US
    - **Pricing Tier:** F0
    - **Resource group:** HappyXamDevs

7. On the **Create** window, click **Create** 

8. In the Azure Portal, navigate to the newly created resource, **HappyXamDevs-ComputerVision**

9. On the **HappyXamDevs-ComputerVision** dashboard, on the left-hand menu, select **Keys**

10. On the **Keys** page, copy the value of **KEY 1**
    > **Note:** We will add the key to our Azure Functions' Application Settings later

9. On the **HappyXamDevs-ComputerVision** dashboard, on the left-hand menu, select **Overview**

10. On the **Overview** page, copy the base url of **Endpoint**
    > **Important:** Only copy the base url of the API endpoint
    >    - Correct Endopint Example: `https://westus.api.cognitive.microsoft.com/`
    >    - Incorrect Endopint Example: `https://westus.api.cognitive.microsoft.com/vision/v1.0`

    > **Note:** We will add the base url to our Azure Functions' Application Settings later

## 2. Creating a Blob Trigger Azure Function

In a previous part, we created an Azure Function with an HTTP Trigger - an Azure Function invoked by call to its HTTP end point. We can also create functions that are triggered when blobs are [saved into Azure Blob Storage](https://docs.microsoft.com/azure/azure-functions/functions-create-storage-blob-triggered-function/?WT.mc_id=mobileappsoftomorrow-workshop-jabenn), similar to how a database trigger is fired on an INSERT in a traditional relational database like SQL Server.

Functions can also have bindings to other Azure resources such as storage and Cosmos DB. These bindings can be input bindings so that data from the resource is passed to the function as an input parameter, or output bindings so that data returned from the function or passed as an `out` parameter can be sent to a resource. For example, you can bind the return value of a function to a Cosmos DB database and have the returned value inserted as a document into the database once the function completes, simply by configuring the bindings.

### Creating the Blob trigger

1. In the [Azure Portal](https://portal.azure.com/?WT.mc_id=mobileappsoftomorrow-workshop-jabenn), navigate to the Azure Function resource, **HappXamDevsFunction-[Your Last Name]**
    - E.g. HappyXamDevsFunction-Minnick

2. On the **Azure Functions** dashboard, on the lef-hand menu, click **Functions**

4. On the **Overview** page, click **Application settings**

5. On the **Application settings** page, in the **Application settings** frame, click **+ Add new setting**

6. On the **Application settings** page, in the **Application settings** frame, enter the following:
    - **App Setting:** ComputerVisionApiKey
    - **Value:** [Your Computer Vision API Key]

5. On the **Application settings** page, in the **Application settings** frame, click **+ Add new setting**

6. On the **Application settings** page, in the **Application settings** frame, enter the following:
    - **App Setting:** ComputerVisionBaseUrl
    - **Value:** [Your Computer Vision Base Url]

7. On the **Application settings** page, at the top, click **Save**

8. On the **Functions** page, click **HappyXamDevsFunction-[Your Last Name]**

9. On the **Functions** page, click **+ Add New Function**

10. On the **Choose a template...** page, select **Azure Blob Storage Trigger** panel and click on _C#_.

11. On the **Azure Blob Storage Trigger** popout, if prompted to install extensions, select **Install**

11. On the **Azure Blob Storage Trigger** popout, if prompted to install extensions, select **Continue**

12. On the **Azure Blob Storage Trigger** popout, enter the following:
    - **Name:** ProcessPhotoFromBlob
    - **Path:** photos/{name}
        > **Note:** `photos/{name}` tells the trigger to listen on any inserted or updated blobs in the `photos` collection, and pass the blob file name into the function as a parameter called `name`
    - **Storage account connection:** AzureWebJobStorage

13. On the **Azure Blob Storage Trigger** popout, click **Create**

14. In the **ProcessPhotoFromBlob** Function page, scroll to the right until **View files** is visible

15. In the **ProcessPhotoFromBlob** Function page, select **View files**

16. In the **View Files** window, click the **+ Add**
17. In the **file name** entry, enter `function.proj`
18. Press the **Return** key on the keyboard to save the new file

    ![Adding a new file to the Azure Function](../Images/PortalAddFileToFunction.png)8
19. In the **function.proj** text editor, enter the following:

```xml
<Project Sdk="Microsoft.NET.Sdk">
    <PropertyGroup>
        <TargetFramework>netstandard2.0</TargetFramework>
    </PropertyGroup>

    <ItemGroup>
        <PackageReference Include="Microsoft.Azure.CognitiveServices.Vision.ComputerVision" Version="3.3.0" />
        <PackageReference Include="WindowsAzure.Storage" Version="9.3.3" />
    </ItemGroup>
</Project>
```

20. In the **function.proj** text editor, click **Save**

21. In the the **View Files** window, select **run.csx**

22. In the **run.csx** editor, enter the following code:

```cs
#r "Microsoft.WindowsAzure.Storage"

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;

public static async Task Run(CloudBlockBlob myBlob, string name, IAsyncCollector<dynamic> documentCollector, ILogger log)
{
    var apiKey = Environment.GetEnvironmentVariable("ComputerVisionApiKey");
    var creds = new ApiKeyServiceClientCredentials(apiKey);

    var visionApi = new ComputerVisionClient(creds)
    {
        Endpoint = Environment.GetEnvironmentVariable("ComputerVisionBaseUrl")
    };

    log.LogInformation("Created Vision API Client");

    using (var stream = new MemoryStream())
    {
        await myBlob.DownloadToStreamAsync(stream);

        var features = new List<VisualFeatureTypes>
        {
            VisualFeatureTypes.Description,
            VisualFeatureTypes.Tags
        };
        var analysis = await visionApi.AnalyzeImageInStreamWithHttpMessagesAsync(stream, features);

        log.LogInformation("Completed Vision Analysis");

        await documentCollector.AddAsync(new
        {
            Name = name,
            Tags = analysis.Body.Tags.Select(t => t.Name).ToArray(),
            Caption = analysis.Body.Description.Captions.FirstOrDefault()?.Text ?? ""
        });
    }

    log.LogInformation("Saved Analysis to Cosmos Db");
}
```

> **About the Code**
>
> `await visionApi.AnalyzeImageInStreamWithHttpMessagesAsync(myBlob, features);` retrieves the the image's `Description` and `Tags` from the machine learning results using the Vision API

## 3. Setting up an output binding to Cosmos DB

Now that you have a function that returns an object with the results of the vision analysis, you will need to bind this output to your Cosmos DB database. Once this binding is set up, every time this trigger runs the returned object will be saved as a JSON document inside your Cosmos DB instance.

1. Under the _ProcessPhotoFromBlob_ node in the left-hand menu, select the _Integrate_ node. You will see the Blob trigger you set up, with nothing configured as input or output bindings.
2. Under _Outputs_, click the "+ New Output" button. Select _Azure Cosmos DB_ and click "Select".
3. For the _Document parameter name_, check the _Use function return value_ option.
4. Set the database name to be "Photos" and collection name to be "PhotoMetadata".
5. Leave the _Partition key_ blank.
6. For the _Azure Cosmos DB Account connection_, click the _new_ option to configure a new connection string. The select your Cosmos DB resource from the list that appears.
7. Click "Save".

    ![Configuring the Cosmos DB output binding](../Images/PortalConfigureCosmosOutputBinding.png)

## Testing it all out

To verify this is working, launch the mobile app on your platform of choice and upload a photo. From the Azure Portal, head to your Cosmos DB resource, and select the _Data Explorer_ from the left-hand menu. From there expand the _Photos_ database and the _PhotosMetadata_ collection and select _Documents_. You should see the new document in the main pane, and when you select it you will see the JSON for this document, showing the Blob name, as well as the caption and tags for this photo from the Computer Vision API.

![Browsing documents in Cosmos DB](../Images/PortalCosmosBrowseDocument.png)

## Next step

Now that you have photos in Blob storage and photo metadata in Cosmos DB, the next step is to [create some Azure Functions to retrieve the photos](./10-FunctionToLoadPhotos.md).