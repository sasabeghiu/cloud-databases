using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models;
using OnlineStore.DTO;
using OnlineStore.Services.Interfaces;

namespace OnlineStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderCommandService _orderCommandService;
        private readonly IOrderQueryService _orderQueryService;

        public OrdersController(IOrderCommandService orderCommandService, IOrderQueryService orderQueryService)
        {
            _orderCommandService = orderCommandService;
            _orderQueryService = orderQueryService;
        }

        public class CreateOrderRequest
        {
            public int UserId { get; set; }
            public List<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            var orderDto = new OrderCreateDto
            {
                UserId = request.UserId,
                OrderItems = request.OrderItems
            };

            await _orderCommandService.CreateOrderAsync(orderDto);
            return StatusCode(201);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders(int pageNumber, int pageSize)
        {
            var orders = await _orderQueryService.GetAllOrdersAsync(pageNumber, pageSize);
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _orderQueryService.GetOrderByIdAsync(id);
            return order != null ? Ok(order) : NotFound();
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetOrdersByUserId(int userId)
        {
            var orders = await _orderQueryService.GetOrdersByUserIdAsync(userId);
            return Ok(orders);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] OrderStatus status)
        {
            await _orderCommandService.UpdateOrderStatusAsync(id, status);
            return NoContent();
        }

        [HttpPut("{id}/ship")]
        public async Task<IActionResult> UpdateShippingDate(int id, [FromBody] DateTime shippingDate)
        {
            await _orderCommandService.UpdateShippingDateAsync(id, shippingDate);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            await _orderCommandService.DeleteOrderAsync(id);
            return NoContent();
        }
    }
}