using Microsoft.AspNetCore.Mvc;
using OnlineStore.DTO;
using OnlineStore.Models;
using OnlineStore.Services.Interfaces;

namespace OnlineStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderCommandService _orderCommandService;
        private readonly IOrderQueryService _orderQueryService;
        private readonly IProductService _productService;

        public OrdersController(
            IOrderCommandService orderCommandService,
            IOrderQueryService orderQueryService,
            IProductService productService
        )
        {
            _orderCommandService = orderCommandService;
            _orderQueryService = orderQueryService;
            _productService = productService;
        }

        public class CreateOrderRequest
        {
            public int UserId { get; set; }
            public List<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            foreach (var orderItem in request.OrderItems)
            {
                var currentProduct = await _productService.GetProductByIdAsync(orderItem.ProductId);
                var newStock = currentProduct.Stock - orderItem.Quantity;

                if (newStock < 0)
                {
                    return BadRequest(
                        $"Not enough stock for Product {orderItem.ProductId}. Current stock: {currentProduct.Stock}, Requested: {orderItem.Quantity}"
                    );
                }
            }

            var orderDto = new OrderCreateDto
            {
                UserId = request.UserId,
                OrderItems = request.OrderItems,
            };

            var order = await _orderCommandService.CreateOrderAsync(orderDto);

            var httpClient = new HttpClient();
            var requestUrl =
                $"https://updateorderprocessedduration.azurewebsites.net/api/orders/{order.OrderId}/update-stock?code=Ofyl3QtgwFoax5ZipDbaIO8Ax5xFIJ9DgHv4z7Ql4YYvAzFuRi0uyg%3D%3D"; // Use the actual URL
            var orderDetails = new OrderCreateDto { OrderItems = orderDto.OrderItems };
            var response = await httpClient.PostAsJsonAsync(requestUrl, orderDto);

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode(500);
            }

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
        public async Task<IActionResult> UpdateShippingDate(
            int id,
            [FromBody] DateTime shippingDate
        )
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
