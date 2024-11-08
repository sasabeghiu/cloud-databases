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

        public ReviewsController(IReviewCommandService reviewCommandService, IReviewQueryService reviewQueryService)
        {
            _reviewCommandService = reviewCommandService;
            _reviewQueryService = reviewQueryService;
        }

        [HttpPost]
        public async Task<IActionResult> AddReview([FromBody] ReviewCreateDto reviewDto)
        {
            await _reviewCommandService.AddReviewAsync(reviewDto);
            return StatusCode(201);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReviewById(int id)
        {
            var review = await _reviewQueryService.GetReviewByIdAsync(id);
            return review != null ? Ok(review) : NotFound();
        }

        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetReviewsByProductId(int productId)
        {
            var reviews = await _reviewQueryService.GetReviewsByProductIdAsync(productId);
            return Ok(reviews);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            await _reviewCommandService.DeleteReviewAsync(id);
            return NoContent();
        }
    }
}