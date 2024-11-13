using Microsoft.Azure.Cosmos;
using OnlineStore.DAL;
using OnlineStore.DTO;
using OnlineStore.Models;
using OnlineStore.Services.Interfaces;

namespace OnlineStore.Services.Implementations
{
    public class ReviewQueryService : IReviewQueryService
    {
        private readonly CosmosDbContext _cosmosContext;

        public ReviewQueryService(CosmosDbContext cosmosContext)
        {
            _cosmosContext = cosmosContext;
        }

        public async Task<IEnumerable<ReviewDto>> GetReviewsByProductIdAsync(int productId)
        {
            var queryDefinition = new QueryDefinition(
                "SELECT * FROM c WHERE c.ProductId = @productId"
            ).WithParameter("@productId", productId);

            var queryRequestOptions = new QueryRequestOptions { };

            var queryIterator = _cosmosContext.ReviewsContainer.GetItemQueryIterator<Review>(
                queryDefinition,
                requestOptions: queryRequestOptions
            );

            var reviews = new List<ReviewDto>();

            while (queryIterator.HasMoreResults)
            {
                var response = await queryIterator.ReadNextAsync();
                reviews.AddRange(
                    response.Select(review => new ReviewDto
                    {
                        ReviewId = review.id,
                        UserId = review.UserId,
                        ProductId = review.ProductId,
                        Content = review.Content,
                        Rating = review.Rating,
                        ReviewDate = review.ReviewDate,
                    })
                );
            }

            return reviews;
        }

        public async Task<ReviewDto> GetReviewByIdAsync(string reviewId, int userId)
        {
            var response = await _cosmosContext.ReviewsContainer.ReadItemAsync<Review>(
                reviewId,
                new PartitionKey(userId)
            );
            var review = response.Resource;

            return new ReviewDto
            {
                ReviewId = review.id,
                UserId = review.UserId,
                ProductId = review.ProductId,
                Content = review.Content,
                Rating = review.Rating,
                ReviewDate = review.ReviewDate,
            };
        }
    }
}
