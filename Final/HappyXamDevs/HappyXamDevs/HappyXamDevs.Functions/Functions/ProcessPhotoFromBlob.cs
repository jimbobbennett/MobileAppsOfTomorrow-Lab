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

namespace HappyXamDevs.Functions
{
    public static class ProcessPhotoFromBlob
    {
        [FunctionName(nameof(ProcessPhotoFromBlob))]
        public static async Task Run([BlobTrigger("photos/{name}")] CloudBlockBlob myBlob, string name,
                                        [CosmosDB(databaseName: "Photos",
                                                    collectionName: "PhotoMetadata",
                                                    ConnectionStringSetting = AzureConstants.CosmosDbConnectionString)]IAsyncCollector<dynamic> documentCollector,
                                        ILogger log)
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
    }
}