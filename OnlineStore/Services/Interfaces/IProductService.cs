using OnlineStore.DTO;

namespace OnlineStore.Services.Interfaces
{
    public interface IProductService
    {
        Task<ProductDto> GetProductByIdAsync(int productId);
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task UpdateProductStockAsync(int productId, int quantity);
        Task CreateProductAsync(ProductCreateDto productDto);
        Task DeleteProductAsync(int productId);
    }
}