using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;

namespace HappyXamDevs.Functions
{
    public static class ProcessPhotoFromBlob
    {
        [FunctionName(nameof(ProcessPhotoFromBlob))]
        public static async Task Run([QueueTrigger(AzureConstants.ProcessBlobQueueName)] string blobName,
                                        [CosmosDB(AzureConstants.CosmosDbDatabaseName,
                                                    AzureConstants.CosmosDbCollectionName,
                                                    ConnectionStringSetting = AzureConstants.CosmosDbConnectionString)]IAsyncCollector<object> documentCollector,
                                        ILogger log)
        {
            log.LogInformation("Starting ProcessPhotoFromBlob");
            log.LogInformation($"{nameof(blobName)}: {blobName}");

            var apiKey = Environment.GetEnvironmentVariable("ComputerVisionApiKey");
            var creds = new ApiKeyServiceClientCredentials(apiKey);

            var visionApi = new ComputerVisionClient(creds)
            {
                Endpoint = Environment.GetEnvironmentVariable("ComputerVisionBaseUrl")
            };

            log.LogInformation("Created Vision API Client");

            var connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            CloudStorageAccount.TryParse(connectionString, out var storageAccount);

            var blobClient = storageAccount.CreateCloudBlobClient();
            var blobContainer = blobClient.GetContainerReference("photos");

            var photoBlob = blobContainer.GetBlockBlobReference(blobName);

            log.LogInformation("Retrieved Blob from Storage");

            var filePath = $"D:\\home\\blobImage{System.DateTime.UtcNow.Ticks}.jpeg";
            using (var fileStream = System.IO.File.OpenWrite(filePath))
            {
                await photoBlob.DownloadToStreamAsync(fileStream);
            }

            using (var fileStream = System.IO.File.OpenRead(filePath))
            {
                var features = new List<VisualFeatureTypes>
                {
                    VisualFeatureTypes.Description,
                    VisualFeatureTypes.Tags
                };

                var analysis = await visionApi.AnalyzeImageInStreamAsync(fileStream, features);
                var tags = analysis.Tags.Select(t => t.Name);
                var caption = analysis.Description.Captions.FirstOrDefault()?.Text ?? "";

                log.LogInformation($"{nameof(caption)}: {caption}");
                foreach (var tag in tags)
                    log.LogInformation($"{nameof(tag)}: {tag}");

                await documentCollector.AddAsync(new
                {
                    Name = blobName,
                    Tags = tags.ToArray(),
                    Caption = caption
                });

                log.LogInformation("Saved Analysis to Cosmos Db");
            }
        }
    }
}