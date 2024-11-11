using OnlineStore.DTO;

namespace OnlineStore.Services.Interfaces
{
    public interface IReviewCommandService
    {
        Task AddReviewAsync(ReviewCreateDto reviewDto);
        Task DeleteReviewAsync(string reviewId, int userId);
    }
}