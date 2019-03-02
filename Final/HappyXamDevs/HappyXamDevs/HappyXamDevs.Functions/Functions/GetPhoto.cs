using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;

namespace HappyXamDevs.Functions
{
    public static class GetPhoto
    {
        [FunctionName(nameof(GetPhoto))]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "photo/{blobName}")]HttpRequestMessage req, string blobName, ILogger log)
        {
            var connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");

            CloudStorageAccount.TryParse(connectionString, out var storageAccount);
            var blobClient = storageAccount.CreateCloudBlobClient();

            var blobContainer = blobClient.GetContainerReference("photos");
            var photoBlob = blobContainer.GetBlockBlobReference(blobName);

            log.LogInformation($"Retrieved Blob, {blobName}");

            var filePath = $"D:\\home\\blobPhoto{DateTime.UtcNow.Ticks}.jpeg";
            using (var fileStream = File.OpenWrite(filePath))
            {
                await photoBlob.DownloadToStreamAsync(fileStream);
            }
            log.LogInformation($"Downloaded Blob");

            using (var fileStream = File.OpenRead(filePath))
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
    }
}
