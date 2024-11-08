using Microsoft.EntityFrameworkCore;
using OnlineStore.DAL;
using OnlineStore.DTO;
using OnlineStore.Models;
using OnlineStore.Services.Interfaces;

namespace OnlineStore.Services.Implementations
{
    public class ReviewQueryService : IReviewQueryService
    {
        private readonly OnlineStoreContext _context;

        public ReviewQueryService(OnlineStoreContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ReviewDto>> GetReviewsByProductIdAsync(int productId)
        {
            var reviews = await _context.Reviews
                .Where(r => r.ProductId == productId)
                .ToListAsync();

            return reviews.Select(review => new ReviewDto
            {
                ReviewId = review.ReviewId,
                UserId = review.UserId,
                ProductId = review.ProductId,
                Content = review.Content,
                Rating = review.Rating,
                ReviewDate = review.ReviewDate
            });
        }

        public async Task<ReviewDto> GetReviewByIdAsync(int reviewId)
        {
            var review = await _context.Reviews.FindAsync(reviewId) ?? throw new KeyNotFoundException($"Review with ID {reviewId} not found.");

            return new ReviewDto
            {
                ReviewId = review.ReviewId,
                UserId = review.UserId,
                ProductId = review.ProductId,
                Content = review.Content,
                Rating = review.Rating,
                ReviewDate = review.ReviewDate
            };
        }
    }
}