using Microsoft.AspNetCore.Mvc;
using OnlineStore.DTO;
using OnlineStore.Services.Interfaces;

namespace OnlineStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewCommandService _reviewCommandService;
        private readonly IReviewQueryService _reviewQueryService;

        public ReviewsController(
            IReviewCommandService reviewCommandService,
            IReviewQueryService reviewQueryService
        )
        {
            _reviewCommandService = reviewCommandService;
            _reviewQueryService = reviewQueryService;
        }

        [HttpPost]
        public async Task<IActionResult> AddReview([FromBody] ReviewCreateDto reviewDto)
        {
            await _reviewCommandService.AddReviewAsync(reviewDto);
            return StatusCode(201, reviewDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReviewById(string id, int userId)
        {
            var review = await _reviewQueryService.GetReviewByIdAsync(id, userId);
            return review != null ? Ok(review) : NotFound();
        }

        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetReviewsByProductId(int productId)
        {
            var reviews = await _reviewQueryService.GetReviewsByProductIdAsync(productId);
            return reviews != null ? Ok(reviews) : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(string id, int userId)
        {
            await _reviewCommandService.DeleteReviewAsync(id, userId);
            return NoContent();
        }
    }
}
