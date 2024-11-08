namespace OnlineStore.DTO
{
    public class ReviewCreateDto
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public string? Content { get; set; }
        public int Rating { get; set; }
    }

    public class ReviewDto
    {
        public int ReviewId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public string? Content { get; set; }
        public int Rating { get; set; }
        public DateTime ReviewDate { get; set; }
    }

}