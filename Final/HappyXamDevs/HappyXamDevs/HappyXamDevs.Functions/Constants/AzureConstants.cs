namespace HappyXamDevs.Functions
{
    public static class AzureConstants
    {
#error Missing Cosmos Db Connection String
        public const string CosmosDbConnectionString = "[Your Cosmos Db Connection String]"

#error Missing Blob Storage Connection String
        public const string BlobStorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=happyxamdevsstoragebrm;AccountKey=WGQdWxyEONgfCsNTaFy/1OHHZZy+UaKgSof2+TJUrsRCPZNu66Yk6u5A62fHPgSrZhrhQ2OEu17KGAbWKjqqOw==;EndpointSuffix=core.windows.net";

        public const string ProcessBlobQueueName = "processblobqueue";
    }
}
