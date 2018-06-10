# Creating functions to load the photos

So far you have an app that can take a photo, check for happy faces then upload the photo to an Azure function that saves it in Blob storage. A trigger is then fired that analyses the photo to get a description and tags, and stores this data in Cosmos DB.

The next step is to show a timeline on the mobile app of all the photos taken. To do this you will need two more APIs, one to retrieve the metadata for all the photos that have been taken, and one to download the Blobs for each photo.

> Having 2 APIs is more efficient as the mobile device can cache images locally and only request new ones that it needs. In a production app you would normally take this further and only return metadata for new photos using some kind of timestamp.

## Creating a function to load metadata for all photos

The first Function to create is the one to return the metadata for all photos. To do this you could write a function that queries Cosmos DB directly, but it is easier to do this with another function binding - this time an input binding. You can bind a Cosmos DB collection to the input parameter of a Function, either returning the whole collection or just a query. For this Function, you will need the entire collection.

### Creating the GetAllPhotos function

1. From the Azure Portal, create a new Function inside your Azure Functions app. This will need to be a C# HTTP trigger. Set the _Name_ as "GetAllPhotos" and the _Authorization level_ as "Anonymous".
2. Head to the _Integrate_ tab and set the _Route Template_ to be "photo".
3. Ensure only GET is checked for the _Selected HTTP methods_ as this function will only need to support this one method. Then click "Save".

    ![Setting the route template and HTTP method for the GetAllPhotos function](../Images/PortalGetAllPhotosIntegrate.png)

    > This route template is the same as the `UploadPhotos` Function, but because the supported HTTP method is different the Azure Functions app is able to call the right Function when this resource is accessed.

### Creating the Cosmos DB input binding

1. Under _Inputs_ in the _Integrate_ tab, click "+ New Input". Select _Azure Cosmos DB_ then click "Select".
2. Set the _Document parameter name_ to be "documents". This is the name of the parameter in your function that the documents will be passed to.
3. Set the database name to be "Photos" and collection name to be "PhotoMetadata".
4. Leave the _Document ID_, _Partition key_ and _SQL Query_ blank.
5. Set the _Azure Cosmos DB account connection_ to be your connection string that you set up when creating the Blob trigger. Then click "Save".

    ![Configuring the Cosmos DB input binding](../Images/PortalConfigureCosmosInputBinding.png)

### Implementing the GetAllPhotos function

Unfortunately just creating this binding doesn't update your function code to reflect the new input, so you will have to make the relevant code change manually.

1. Select the top-level _GetAllPhotos_ function node to view the code for the function.
2. Change the function parameters to include an `IEnumerable<dynamic>`. Using a `dynamic` here means you do not have to define an explicit type that matches your documents and allows you to change the document structure at any time without re-writing this function. The type is immaterial anyway, as it will be automatically converted to JSON when returned.

    ```cs
    public static HttpResponseMessage Run(HttpRequestMessage req, IEnumerable<dynamic> documents, TraceWriter log)
    ```

3. Delete the existing contents of this function, and add a simple return statement to return a new response wrapping the collection of documents. Doing this will cause the documents to be serialized to JSON automatically and returned as the body of the HTTP response. Then save the function.

    ```cs
    return req.CreateResponse(HttpStatusCode.OK, documents);
    ```

## Creating a function to load a photo

Next you need a function that will take the name of a Blob and return that Blob, encoded as Base64 to allow it to be returned in a JSON object. Once again, you can use an input binding to bind to Blob storage, even providing a way to automatically take an input parameter passed to the function as a REST resource identifier.

### Creating the GetPhoto function

This function will be routed to the `photo/{name}` REST resource, so making an HTTP GET method call to `https://<YourFunctionApp>.azurewebsites.net/api/photo/<photo name>` with the photo name set to the name of the Blob (taken from the Cosmos DB document) will return that blob.

1. From the Azure Portal, create a new function inside your function app. This will need to be a C# HTTP trigger. Set the _Name_ as "GetPhoto" and the _Authorization level_ as "Anonymous".
2. Head to the _Integrate_ tab and set the _Route Template_ to "photo/{name}". This will allow you to add a parameter called `name` to your function and have this automatically populated with the resource name from the URL.
3. Set the _Selected HTTP Methods_ to GET.

    ![Setting the route template and HTTP method for the GetPhoto function](../Images/PortalGetPhotoIntegrate.png)

### Creating the Blob input binding

1. From the _Integrate Tab_, click the "+ New Input" button under _Inputs_, select _Azure Blob Storage_ and click "Select".
2. Set the _Blob parameter name_ to be "blob". This is the name of the parameter in your function that the Blob will be passed to.
3. Set the _Path_ to be "photos/{name}". This maps to the `photos` Blob collection, and will return the Blob whose name matches the value passed as the `name` from the REST resource.
4. Set the _Storage account connection_ to be your Blob storage connection string. Then click "Save".

    ![Configuring the Blob storage input binding](../Images/PortalConfigureBlobsInputBinding.png)

### Implementing the GetPhoto function

1. Head to the Azure Function code, and update the Function signature to include the new binding and route template parameter by adding two new parameters - a `string` called `name` and a `Stream` called `blob`. These parameters will be populated by the photo name from the URL and the Blob from the input binding.

    ```cs
    public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, string name, Stream blob, TraceWriter log)
    ```

2. Read the contents of the Blob stream and encode it as a Base64 string.

    ```cs
    var bytes = new byte[blob.Length];
    await blob.ReadAsync(bytes, 0, Convert.ToInt32(blob.Length));
    var photo = Convert.ToBase64String(bytes);
    ```

3. Create a new anonymous object containing the photo and return it as part of the HTTP response. Then save the function.

    ```cs
    var retVal = new
    {
        Photo = photo
    };
    return req.CreateResponse(HttpStatusCode.OK, retVal);
    ```

The complete code for this function is below.

```cs
using System.Net;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, string name, Stream blob, TraceWriter log)
{
    var bytes = new byte[blob.Length];
    await blob.ReadAsync(bytes, 0, Convert.ToInt32(blob.Length));
    var photo = Convert.ToBase64String(bytes);

    var retVal = new
    {
        Photo = photo
    };
    return req.CreateResponse(HttpStatusCode.OK, retVal);
}
```

## Next step

Now that you have functions to download photo metadata and individual photos, the next step is to [use these in your Xamarin app to download photos](./11-DownloadPhotosToMobileApp.md).