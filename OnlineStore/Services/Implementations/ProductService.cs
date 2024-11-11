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

        public ProductService(OnlineStoreContext context)
        {
            _context = context;
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
                Image = product.Image,
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

        public async Task CreateProductAsync(ProductCreateDto productDto)
        {
            var product = new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Image = productDto.Image,
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
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}