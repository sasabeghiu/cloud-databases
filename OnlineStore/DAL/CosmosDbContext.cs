using Microsoft.Azure.Cosmos;

namespace OnlineStore.DAL
{
    public class CosmosDbContext
    {
        private readonly CosmosClient _cosmosClient;
        public Container ReviewsContainer { get; }

        public CosmosDbContext(
            CosmosClient cosmosClient,
            string databaseName,
            string reviewsContainerName
        )
        {
            _cosmosClient = cosmosClient;

            var database = _cosmosClient.GetDatabase(databaseName);
            ReviewsContainer = database.GetContainer(reviewsContainerName);
        }
    }
}
