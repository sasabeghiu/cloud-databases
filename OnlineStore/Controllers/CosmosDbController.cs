using Microsoft.AspNetCore.Mvc;
using OnlineStore.DTO;
using OnlineStore.Models;
using OnlineStore.Services.Interfaces;

namespace OnlineStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CosmosDbController : ControllerBase
    {
        private readonly ICosmosDbService _cosmosDbService;

        public CosmosDbController(ICosmosDbService cosmosDbService)
        {
            _cosmosDbService = cosmosDbService;
        }

        [HttpPost("add/{containerName}")]
        public async Task<IActionResult> AddItem(string containerName, [FromBody] OrderDto order)
        {
            try
            {
                await _cosmosDbService.AddItemAsync(order, order.UserId.ToString(), containerName);
                return Ok("Item added successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding item to Cosmos DB: {ex.Message}");
                return StatusCode(500, "Failed to add item");
            }
        }

        [HttpGet("get/{containerName}/{id}")]
        public async Task<IActionResult> GetItem(string containerName, string id, [FromQuery] string partitionKey)
        {
            var item = await _cosmosDbService.GetItemAsync<Order>(id, partitionKey, containerName);
            return item == null ? NotFound() : Ok(item);
        }

        [HttpGet("test/{containerName}")]
        public async Task<IActionResult> TestConnection(string containerName, [FromQuery] string partitionKey)
        {
            try
            {
                var items = await _cosmosDbService.GetItemsAsync<OrderDto>("SELECT * FROM c", partitionKey, containerName);
                return Ok(items);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving items: {ex.Message}");
                return StatusCode(500, "Failed to retrieve items");
            }
        }
    }
}
