using System.Collections.Generic;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace HappyXamDevs.Functions
{
    public static class GetAllPhotosMetadata
    {
        [FunctionName(nameof(GetAllPhotosMetadata))]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "photo")]HttpRequestMessage req,
                                            [CosmosDB(AzureConstants.CosmosDbDatabaseName,
                                                    AzureConstants.CosmosDbCollectionName,
                                                    ConnectionStringSetting = AzureConstants.CosmosDbConnectionString)]IEnumerable<dynamic> documents, 
                                            ILogger log)
        {
            return new OkObjectResult(documents);
        }
    }
}
