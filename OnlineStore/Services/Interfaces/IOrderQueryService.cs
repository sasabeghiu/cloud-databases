using OnlineStore.DTO;
using OnlineStore.Models;

namespace OnlineStore.Services.Interfaces
{
    public interface IOrderQueryService
    {
        Task<OrderDto> GetOrderByIdAsync(int orderId);
        Task<IEnumerable<OrderDto>> GetOrdersByUserIdAsync(int userId);
        Task<IEnumerable<OrderDto>> GetOrdersByStatusAsync(OrderStatus status);
        Task<IEnumerable<OrderDto>> GetAllOrdersAsync(int pageNumber, int pageSize);
        Task<IEnumerable<OrderDto>> GetPendingOrdersAsync();
    }
}
