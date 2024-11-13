using OnlineStore.DTO;
using OnlineStore.Models;

namespace OnlineStore.Services.Interfaces
{
    public interface IOrderCommandService
    {
        Task<Order> CreateOrderAsync(OrderCreateDto orderDto);
        Task UpdateOrderStatusAsync(int orderId, OrderStatus status);
        Task UpdateShippingDateAsync(int orderId, DateTime shippingDate);
        Task UpdateOrderProcessedDurationAsync(int orderId, string processedDuration);
        Task DeleteOrderAsync(int orderId);
    }
}
