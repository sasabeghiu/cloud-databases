using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace OnlineStore.Models
{
    public class Review
    {
        public string id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [JsonProperty("UserId")]
        public int UserId { get; set; }

        [JsonIgnore]
        public User? User { get; set; }

        [Required]
        public int ProductId { get; set; }

        [JsonIgnore]
        public Product? Product { get; set; }

        [Required]
        [StringLength(500)]
        public string? Content { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        public DateTime ReviewDate { get; set; }
    }
}
