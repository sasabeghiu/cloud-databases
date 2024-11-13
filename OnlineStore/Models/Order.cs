using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        [Required]
        public int UserId { get; set; }
        public User? User { get; set; }

        public DateTime OrderDate { get; set; }
        public DateTime? ShippingDate { get; set; }
        public OrderStatus Status { get; set; }
        public string? ProcessedDuration { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }

    public enum OrderStatus
    {
        Ordered,
        Shipped,
        Delivered,
        Canceled
    }
}
