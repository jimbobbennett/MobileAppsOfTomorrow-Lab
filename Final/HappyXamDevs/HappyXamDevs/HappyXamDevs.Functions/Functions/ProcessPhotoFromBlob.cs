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

namespace HappyXamDevs.Functions
{
    public static class ProcessPhotoFromBlob
    {
        [FunctionName(nameof(ProcessPhotoFromBlob))]
        public static async Task Run(ILogger log, [BlobTrigger("photos/{name}")] Stream myBlob, string name,
                                        [CosmosDB(databaseName: "Photos",
                                                    collectionName: "PhotoMetadata",
                                                    ConnectionStringSetting = AzureConstants.CosmosDbConnectionString)]IAsyncCollector<dynamic> documentCollector)
        {
            var apiKey = Environment.GetEnvironmentVariable("ComputerVisionApiKey");
            var creds = new ApiKeyServiceClientCredentials(apiKey);

            var visionApi = new ComputerVisionClient(creds)
            {
                Endpoint = Environment.GetEnvironmentVariable("ComputerVisionBaseUrl")
            };

            var features = new List<VisualFeatureTypes>
            {
                VisualFeatureTypes.Description,
                VisualFeatureTypes.Tags
            };
            var analysis = await visionApi.AnalyzeImageInStreamWithHttpMessagesAsync(myBlob, features);

            await documentCollector.AddAsync(new
            {
                Name = name,
                Tags = analysis.Body.Tags.Select(t => t.Name).ToArray(),
                Caption = analysis.Body.Description.Captions.FirstOrDefault()?.Text ?? ""
            });
        }
    }
}