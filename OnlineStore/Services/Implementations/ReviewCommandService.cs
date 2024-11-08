using OnlineStore.DAL;
using OnlineStore.DTO;
using OnlineStore.Models;
using OnlineStore.Services.Interfaces;

namespace OnlineStore.Services.Implementations
{
    public class ReviewCommandService : IReviewCommandService
    {
        private readonly OnlineStoreContext _context;

        public ReviewCommandService(OnlineStoreContext context)
        {
            _context = context;
        }

        public async Task AddReviewAsync(ReviewCreateDto reviewDto)
        {
            var review = new Review
            {
                UserId = reviewDto.UserId,
                ProductId = reviewDto.ProductId,
                Content = reviewDto.Content,
                Rating = reviewDto.Rating,
                ReviewDate = DateTime.UtcNow
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteReviewAsync(int reviewId)
        {
            var review = await _context.Reviews.FindAsync(reviewId);
            if (review != null)
            {
                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();
            }
        }
    }
}