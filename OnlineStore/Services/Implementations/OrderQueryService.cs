using Microsoft.EntityFrameworkCore;
using OnlineStore.DAL;
using OnlineStore.DTO;
using OnlineStore.Models;
using OnlineStore.Services.Interfaces;

namespace OnlineStore.Services.Implementations
{
    public class OrderQueryService : IOrderQueryService
    {
        private readonly OnlineStoreContext _context;

        public OrderQueryService(OnlineStoreContext context)
        {
            _context = context;
        }

        public async Task<OrderDto> GetOrderByIdAsync(int orderId)
        {
            var order =
                await _context
                    .Orders.Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                    .FirstOrDefaultAsync(o => o.OrderId == orderId)
                ?? throw new KeyNotFoundException($"Order with ID {orderId} not found.");

            return new OrderDto
            {
                OrderId = order.OrderId,
                UserId = order.UserId,
                OrderDate = order.OrderDate,
                ShippingDate = order.ShippingDate,
                Status = order.Status.ToString(),
                ProcessedDuration = order.ProcessedDuration,
                TotalPrice = order.TotalPrice,
                OrderItems = order
                    .OrderItems.Select(oi => new OrderItemDto
                    {
                        ProductId = oi.ProductId,
                        Quantity = oi.Quantity,
                    })
                    .ToList(),
            };
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersByUserIdAsync(int userId)
        {
            var orders = await _context
                .Orders.Where(o => o.UserId == userId)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ToListAsync();

            return orders.Select(order => new OrderDto
            {
                OrderId = order.OrderId,
                UserId = order.UserId,
                OrderDate = order.OrderDate,
                ShippingDate = order.ShippingDate,
                Status = order.Status.ToString(),
                ProcessedDuration = order.ProcessedDuration,
                TotalPrice = order.TotalPrice,
                OrderItems = order
                    .OrderItems.Select(oi => new OrderItemDto
                    {
                        ProductId = oi.ProductId,
                        Quantity = oi.Quantity,
                    })
                    .ToList(),
            });
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersByStatusAsync(OrderStatus status)
        {
            var orders = await _context
                .Orders.Where(o => o.Status == status)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ToListAsync();

            return orders.Select(order => new OrderDto
            {
                OrderId = order.OrderId,
                UserId = order.UserId,
                OrderDate = order.OrderDate,
                ShippingDate = order.ShippingDate,
                Status = order.Status.ToString(),
                ProcessedDuration = order.ProcessedDuration,
                TotalPrice = order.TotalPrice,
                OrderItems = order
                    .OrderItems.Select(oi => new OrderItemDto
                    {
                        ProductId = oi.ProductId,
                        Quantity = oi.Quantity,
                    })
                    .ToList(),
            });
        }

        public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync(int pageNumber, int pageSize)
        {
            var orders = await _context
                .Orders.Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .OrderBy(o => o.OrderDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return orders.Select(order => new OrderDto
            {
                OrderId = order.OrderId,
                UserId = order.UserId,
                OrderDate = order.OrderDate,
                ShippingDate = order.ShippingDate,
                Status = order.Status.ToString(),
                ProcessedDuration = order.ProcessedDuration,
                TotalPrice = order.TotalPrice,
                OrderItems = order
                    .OrderItems.Select(oi => new OrderItemDto
                    {
                        ProductId = oi.ProductId,
                        Quantity = oi.Quantity,
                    })
                    .ToList(),
            });
        }

        public async Task<IEnumerable<OrderDto>> GetPendingOrdersAsync()
        {
            var orders = await _context
                .Orders.Where(o => o.ProcessedDuration == null && o.Status != OrderStatus.Canceled)
                .ToListAsync();

            return orders.Select(order => new OrderDto
            {
                OrderId = order.OrderId,
                UserId = order.UserId,
                OrderDate = order.OrderDate,
                ShippingDate = order.ShippingDate,
                Status = order.Status.ToString(),
                ProcessedDuration = order.ProcessedDuration,
                TotalPrice = order.TotalPrice,
                OrderItems = order
                    .OrderItems.Select(oi => new OrderItemDto
                    {
                        ProductId = oi.ProductId,
                        Quantity = oi.Quantity,
                    })
                    .ToList(),
            });
        }
    }
}
