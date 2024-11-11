using Microsoft.EntityFrameworkCore;
using OnlineStore.DAL;
using OnlineStore.DTO;
using OnlineStore.Models;
using OnlineStore.Services.Interfaces;

namespace OnlineStore.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly OnlineStoreContext _context;
        private readonly BlobStorageService _blobStorageService;

        public ProductService(OnlineStoreContext context, BlobStorageService blobStorageService)
        {
            _context = context;
            _blobStorageService = blobStorageService;
        }

        public async Task<ProductDto> GetProductByIdAsync(int productId)
        {
            var product = await _context.Products
                // .Include(p => p.Reviews)
                .FirstOrDefaultAsync(p => p.ProductId == productId) ?? throw new KeyNotFoundException($"Product with ID {productId} not found.");

            return new ProductDto
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Description = product.Description,
                Image = product.Image,
                Price = product.Price,
                Stock = product.Stock
            };
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _context.Products.ToListAsync();

            return products.Select(product => new ProductDto
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Description = product.Description,
                Image = product.Image + "?sv=2018-03-28&st=2024-11-11T18%3A44%3A18Z&se=2024-11-12T18%3A44%3A18Z&sr=c&sp=rl&sig=%2FwNGlXDaIcwjjQgIpN4dm%2BxlqH6sFOhMkxFX8LF4xqU%3D",
                Price = product.Price,
                Stock = product.Stock
            });
        }

        public async Task UpdateProductStockAsync(int productId, int quantity)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product != null)
            {
                product.Stock = quantity;
                await _context.SaveChangesAsync();
            }
        }

        public async Task CreateProductAsync(ProductCreateDto productDto, Stream imageStream)
        {
            string? imageUrl = null;

            if (imageStream != null)
            {
                // Upload the image to blob storage and get the URL
                imageUrl = await _blobStorageService.UploadImageAsync(imageStream, $"{productDto.Name}_{DateTime.UtcNow}.jpg");
            }

            var product = new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Image = imageUrl,
                Price = productDto.Price,
                Stock = productDto.Stock
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product != null)
            {
                if (!string.IsNullOrEmpty(product.Image))
                {
                    var fileName = Path.GetFileName(new Uri(product.Image).LocalPath);
                    await _blobStorageService.DeleteImageAsync(fileName);
                }

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}