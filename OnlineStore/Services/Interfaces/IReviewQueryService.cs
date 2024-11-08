using OnlineStore.DTO;

namespace OnlineStore.Services.Interfaces
{
    public interface IReviewQueryService
    {
        Task<IEnumerable<ReviewDto>> GetReviewsByProductIdAsync(int productId);
        Task<ReviewDto> GetReviewByIdAsync(int reviewId);
    }
}