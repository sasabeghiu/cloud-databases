using OnlineStore.DTO;
using OnlineStore.Models;

namespace OnlineStore.Services.Interfaces
{
    public interface IOrderCommandService
    {
        Task CreateOrderAsync(OrderCreateDto orderDto);
        Task UpdateOrderStatusAsync(int orderId, OrderStatus status);
        Task UpdateShippingDateAsync(int orderId, DateTime shippingDate);
        Task DeleteOrderAsync(int orderId);
    }
}