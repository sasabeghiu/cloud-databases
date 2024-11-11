using Microsoft.Azure.Cosmos;
using OnlineStore.DAL;
using OnlineStore.DTO;
using OnlineStore.Models;
using OnlineStore.Services.Interfaces;

namespace OnlineStore.Services.Implementations
{
    public class ReviewCommandService : IReviewCommandService
    {
        private readonly CosmosDbContext _cosmosContext;

        public ReviewCommandService(CosmosDbContext cosmosContext)
        {
            _cosmosContext = cosmosContext;
        }

        public async Task AddReviewAsync(ReviewCreateDto reviewDto)
        {
            var review = new Review
            {
                id = Guid.NewGuid().ToString(),
                UserId = reviewDto.UserId,
                ProductId = reviewDto.ProductId,
                Content = reviewDto.Content,
                Rating = reviewDto.Rating,
                ReviewDate = DateTime.UtcNow
            };

            await _cosmosContext.ReviewsContainer.CreateItemAsync(review);
        }

        public async Task DeleteReviewAsync(string reviewId, int userId)
        {
            await _cosmosContext.ReviewsContainer.DeleteItemAsync<Review>(reviewId, new PartitionKey(userId));
        }

    }
}