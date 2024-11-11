using Microsoft.AspNetCore.Mvc;
using OnlineStore.DTO;
using OnlineStore.Services.Interfaces;

namespace OnlineStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductCreateDto productDto)
        {
            if (productDto == null)
            {
                return BadRequest("Product details are required.");
            }

            Stream? imageStream = null;

            try
            {
                if (!string.IsNullOrEmpty(productDto.ImageUrl))
                {
                    using var httpClient = new HttpClient();
                    var imageBytes = await httpClient.GetByteArrayAsync(productDto.ImageUrl);
                    imageStream = new MemoryStream(imageBytes);
                }
            }
            catch (HttpRequestException)
            {
                return BadRequest("Failed to download the image from the provided URL.");
            }

            await _productService.CreateProductAsync(productDto, imageStream ?? Stream.Null);
            return StatusCode(201);
        }


        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound($"Product with ID {id} not found");
            }

            return Ok(product);
        }

        [HttpPut("{id}/stock")]
        public async Task<IActionResult> UpdateProductStock(int id, [FromBody] int quantity)
        {
            await _productService.UpdateProductStockAsync(id, quantity);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _productService.DeleteProductAsync(id);
            return NoContent();
        }
    }
}