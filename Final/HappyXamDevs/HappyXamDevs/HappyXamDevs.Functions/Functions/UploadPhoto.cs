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

namespace UploadPhoto
{
    public static class UploadPhoto
    {
        [FunctionName(nameof(UploadPhoto))]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage req, ILogger log)
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
    }
}
