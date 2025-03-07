namespace OnlineStore.DTO
{
    public class OrderCreateDto
    {
        public int UserId { get; set; }
        public List<OrderItemDto>? OrderItems { get; set; }
    }

    public class OrderDto
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? ShippingDate { get; set; }
        public string? Status { get; set; }
        public string? ProcessedDuration { get; set; }
        public decimal TotalPrice { get; set; }
        public List<OrderItemDto>? OrderItems { get; set; }
    }

    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
