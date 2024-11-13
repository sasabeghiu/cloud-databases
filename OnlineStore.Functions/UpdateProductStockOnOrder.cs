using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OnlineStore.Services.Interfaces;

namespace OnlineStore.Functions
{
    public class UpdateProductStockOnOrder
    {
        private readonly IProductService _productService;
        private readonly IOrderQueryService _orderQueryService;

        public UpdateProductStockOnOrder(
            IProductService productService,
            IOrderQueryService orderQueryService
        )
        {
            _productService = productService;
            _orderQueryService = orderQueryService;
        }

        [FunctionName("UpdateProductStockOnOrder")]
        public async Task Run(
            [HttpTrigger(
                AuthorizationLevel.Function,
                "post",
                Route = "orders/{orderId}/update-stock"
            )]
                HttpRequest req,
            int orderId,
            ILogger log
        )
        {
            log.LogInformation(
                $"HTTP trigger to update product stock triggered for OrderId: {orderId}"
            );

            // Fetch the order based on OrderId
            var order = await _orderQueryService.GetOrderByIdAsync(orderId); // Assuming a method to get order by Id

            if (order == null || order.OrderItems == null)
            {
                log.LogError("No order items found for the specified order.");
                return;
            }

            // Loop through each order item
            foreach (var orderItem in order.OrderItems)
            {
                // Get the current stock for the product
                var currentProduct = await _productService.GetProductByIdAsync(orderItem.ProductId);

                if (currentProduct != null)
                {
                    // Update the stock by deducting the quantity ordered
                    var newStock = currentProduct.Stock - orderItem.Quantity;
                    if (newStock < 0)
                    {
                        log.LogError(
                            $"Not enough stock for Product {orderItem.ProductId}. Current stock: {currentProduct.Stock}, Requested: {orderItem.Quantity}"
                        );
                        continue;
                    }

                    // Update product stock
                    await _productService.UpdateProductStockAsync(orderItem.ProductId, newStock);
                    log.LogInformation(
                        $"Updated stock for Product {orderItem.ProductId}. New stock: {newStock}"
                    );
                }
                else
                {
                    log.LogError($"Product with ID {orderItem.ProductId} not found.");
                }
            }
        }
    }
}
