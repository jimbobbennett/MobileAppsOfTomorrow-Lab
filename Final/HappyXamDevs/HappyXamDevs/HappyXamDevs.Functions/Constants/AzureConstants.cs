namespace HappyXamDevs.Functions
{
    public static class AzureConstants
    {
#error Missing Cosmos Db Connection String
        public const string CosmosDbConnectionString = "[Your Cosmos DB Connection String]";

        public const string ProcessBlobQueueName = "processblobqueue";

        public const string CosmosDbDatabaseName = "Photos";
        public const string CosmosDbCollectionName = "PhotoMetadata";
    }
}
