using Microsoft.Azure.Cosmos;

namespace OnlineStore.DAL
{
    public class CosmosDbContext
    {
        private readonly CosmosClient _cosmosClient;

        // public Container OrdersContainer { get; }
        public Container ReviewsContainer { get; }

        public CosmosDbContext(
            CosmosClient cosmosClient,
            string databaseName,
            string ordersContainerName,
            string reviewsContainerName
        )
        {
            _cosmosClient = cosmosClient;

            var database = _cosmosClient.GetDatabase(databaseName);
            // OrdersContainer = database.GetContainer(ordersContainerName);
            ReviewsContainer = database.GetContainer(reviewsContainerName);
        }
    }
}
