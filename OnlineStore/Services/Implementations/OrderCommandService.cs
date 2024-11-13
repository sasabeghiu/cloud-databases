using Microsoft.EntityFrameworkCore;
using OnlineStore.DAL;
using OnlineStore.DTO;
using OnlineStore.Models;
using OnlineStore.Services.Interfaces;

namespace OnlineStore.Services.Implementations
{
    public class OrderCommandService : IOrderCommandService
    {
        private readonly OnlineStoreContext _context;

        public OrderCommandService(OnlineStoreContext context)
        {
            _context = context;
        }

        public async Task<Order> CreateOrderAsync(OrderCreateDto orderDto)
        {
            if (orderDto == null)
            {
                throw new ArgumentNullException(nameof(orderDto));
            }

            var order = new Order
            {
                UserId = orderDto.UserId,
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Ordered,
                OrderItems = new List<OrderItem>(),
            };

            decimal totalPrice = 0;

            foreach (var itemDto in orderDto.OrderItems ?? new List<OrderItemDto>())
            {
                var product = await _context.Products.FirstOrDefaultAsync(p =>
                    p.ProductId == itemDto.ProductId
                );

                if (product == null)
                {
                    throw new KeyNotFoundException(
                        $"Product with ID {itemDto.ProductId} not found."
                    );
                }

                var orderItem = new OrderItem
                {
                    ProductId = itemDto.ProductId,
                    Quantity = itemDto.Quantity,
                    Price = product.Price * itemDto.Quantity,
                };

                totalPrice += orderItem.Price;
                order.OrderItems.Add(orderItem);
            }

            order.TotalPrice = totalPrice;

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return order;
        }

        public async Task UpdateOrderStatusAsync(int orderId, OrderStatus status)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order != null)
            {
                order.Status = status;
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateShippingDateAsync(int orderId, DateTime shippingDate)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order != null)
            {
                order.ShippingDate = shippingDate;
                order.Status = OrderStatus.Shipped;
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateOrderProcessedDurationAsync(int orderId, string processedDuration)
        {
            var order = await _context.Orders.FindAsync(orderId);

            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }

            order.ProcessedDuration = processedDuration.ToString();
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrderAsync(int orderId)
        {
            var order = await _context
                .Orders.Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order != null)
            {
                _context.OrderItems.RemoveRange(order.OrderItems);
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }
    }
}
