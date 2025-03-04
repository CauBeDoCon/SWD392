using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWD392.DTOs;
using SWD392.Repositories;
using System.Security.Claims;

namespace SWD392.Controllers
{
    [Route("api/reviews")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewRepository _reviewRepository;

        public ReviewController(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReviewById(int id)
        {
            var response = await _reviewRepository.GetReviewById(id);
            if (!response.Success)
                return NotFound(new { message = response.Message });
            return Ok(response);
        }

        [Authorize]
        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetReviewsByProduct(int productId)
        {
            var response = await _reviewRepository.GetReviewsByProduct(productId);
            if (!response.Success)
                return NotFound(new { message = response.Message });
            return Ok(response);
        }

        [Authorize]
        [HttpPost("postreviewbyorderdetail/{orderDetailId}")]
        public async Task<IActionResult> CreateReview(int orderDetailId, [FromBody] ReviewDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized(new { message = "Không tìm thấy user." });

            var response = await _reviewRepository.CreateReviewAsync(orderDetailId, dto, userId);
            if (!response.Success)
                return BadRequest(new { message = response.Message });

            return CreatedAtAction(nameof(GetReviewById), new { id = response.Data }, new { Id = response.Data });
        }

        [Authorize(Roles = "Customer")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReview(int id, [FromBody] UpdateReviewDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized(new { message = "Không tìm thấy user." });

            var response = await _reviewRepository.UpdateReviewAsync(id, dto, userId);
            if (!response.Success)
                return BadRequest(new { message = response.Message });

            return NoContent();
        }

        [Authorize(Roles = "Customer")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized(new { message = "Không tìm thấy user." });

            var response = await _reviewRepository.DeleteReviewAsync(id, userId);
            if (!response.Success)
                return BadRequest(new { message = response.Message });

            return NoContent();
        }
    }
}
