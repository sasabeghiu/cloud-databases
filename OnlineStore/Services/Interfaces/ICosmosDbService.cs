namespace OnlineStore.Services.Interfaces
{
    public interface ICosmosDbService
    {
        Task AddItemAsync<T>(T item, string partitionKey, string containerName);
        Task<T?> GetItemAsync<T>(string id, string partitionKey, string containerName);
        Task<IEnumerable<T>> GetItemsAsync<T>(
            string query,
            string partitionKey,
            string containerName
        );
        Task UpdateItemAsync<T>(string id, T item, string partitionKey, string containerName);
        Task DeleteItemAsync<T>(string id, string partitionKey, string containerName);
    }
}
