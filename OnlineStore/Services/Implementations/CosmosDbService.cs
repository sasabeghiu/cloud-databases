using Microsoft.Azure.Cosmos;
using OnlineStore.Models;
using OnlineStore.Services.Interfaces;

namespace OnlineStore.Services.Implementations
{
    public class CosmosDbService : ICosmosDbService
    {
        private readonly CosmosClient _client;
        private readonly string _databaseName;

        public CosmosDbService(CosmosClient client, string databaseName)
        {
            _client = client;
            _databaseName = databaseName;
        }

        // Helper method to get the specified container
        private async Task<Container> GetContainerAsync(string containerName)
        {
            var database = await _client.CreateDatabaseIfNotExistsAsync(_databaseName);
            var containerResponse = await database.Database.CreateContainerIfNotExistsAsync(containerName, "/userId");
            return containerResponse.Container; 
        }

        public async Task AddItemAsync<T>(T item, string partitionKey, string containerName)
        {
            var container = await GetContainerAsync(containerName);

            using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            try
            {
                await container.CreateItemAsync(item, new PartitionKey(partitionKey), cancellationToken: cancellationTokenSource.Token);
                Console.WriteLine("Item added to Cosmos DB.");
            }
            catch (CosmosException ex)
            {
                Console.WriteLine($"Cosmos DB error: {ex.StatusCode} - {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General error: {ex.Message}");
            }
        }


        public async Task<T?> GetItemAsync<T>(string id, string partitionKey, string containerName)
        {
            var container = await GetContainerAsync(containerName);
            Console.WriteLine($"Attempting to retrieve item with ID: {id} and Partition Key: {partitionKey} in Container: {containerName}");

            try
            {
                var response = await container.ReadItemAsync<T>(id, new PartitionKey(partitionKey));
                Console.WriteLine("Item successfully retrieved.");
                return response.Resource;
            }
            catch (CosmosException e) when (e.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                Console.WriteLine("Item not found.");
                return default;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
        }


        public async Task<IEnumerable<T>> GetItemsAsync<T>(string query, string partitionKey, string containerName)
        {
            var container = await GetContainerAsync(containerName);
            var queryDefinition = new QueryDefinition(query);
            var queryRequestOptions = new QueryRequestOptions
            {
                PartitionKey = new PartitionKey(partitionKey)
            };
            var queryResultSetIterator = container.GetItemQueryIterator<T>(queryDefinition, requestOptions: queryRequestOptions);

            List<T> results = new List<T>();

            try
            {
                while (queryResultSetIterator.HasMoreResults)
                {
                    var response = await queryResultSetIterator.ReadNextAsync();
                    results.AddRange(response.ToList());
                }
            }
            catch (CosmosException ex)
            {
                Console.WriteLine($"Cosmos DB Error: {ex.StatusCode} - {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}");
            }

            return results;
        }

        public async Task UpdateItemAsync<T>(string id, T item, string partitionKey, string containerName)
        {
            var container = await GetContainerAsync(containerName);
            await container.UpsertItemAsync(item, new PartitionKey(partitionKey));
        }

        public async Task DeleteItemAsync<T>(string id, string partitionKey, string containerName)
        {
            var container = await GetContainerAsync(containerName);
            await container.DeleteItemAsync<T>(id, new PartitionKey(partitionKey));
        }
    }
}
