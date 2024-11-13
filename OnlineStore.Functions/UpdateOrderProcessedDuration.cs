using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using OnlineStore.Services.Interfaces;

namespace OnlineStore.Functions
{
    public class UpdateOrderProcessedDuration
    {
        private readonly IOrderQueryService _orderQueryService;
        private readonly IOrderCommandService _orderCommandService;

        public UpdateOrderProcessedDuration(
            IOrderQueryService orderQueryService,
            IOrderCommandService orderCommandService
        )
        {
            _orderQueryService = orderQueryService;
            _orderCommandService = orderCommandService;
        }

        [FunctionName("UpdateOrderProcessedDuration")]
        public async Task Run([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation(
                $"UpdateOrderProcessedDuration function executed at: {DateTime.Now}"
            );

            // Retrieve orders that need processing
            var pendingOrders = await _orderQueryService.GetPendingOrdersAsync();
            foreach (var order in pendingOrders)
            {
                if (order.ShippingDate != null)
                {
                    var processedDuration = order.ShippingDate.Value - order.OrderDate;

                    // Format the duration to human-readable format
                    var formattedDuration = FormatProcessedDuration(processedDuration);

                    // Save it to the database
                    await _orderCommandService.UpdateOrderProcessedDurationAsync(
                        order.OrderId,
                        formattedDuration
                    );

                    log.LogInformation(
                        $"Order {order.OrderId} processed duration updated to {formattedDuration}"
                    );
                }
            }
        }

        private string FormatProcessedDuration(TimeSpan duration)
        {
            var days = duration.Days;
            var hours = duration.Hours;
            var minutes = duration.Minutes;
            var seconds = duration.Seconds;

            return $"{days} days, {hours} hours, {minutes} minutes, {seconds} seconds";
        }
    }
}
