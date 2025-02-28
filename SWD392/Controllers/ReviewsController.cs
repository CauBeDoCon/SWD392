using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWD392.DTOs;
using SWD392.Models;
using SWD392.Repositories;

namespace SWD392.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewRepository _reviewRepo;

        public ReviewsController(IReviewRepository repo)
        {
            _reviewRepo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllReviews([FromQuery] int? pageNumber, [FromQuery] int? pageSize)
        {
            int currentPage = pageNumber ?? 1;
            int currentSize = pageSize ?? 10;

            var result = await _reviewRepo.GetAllReviewsAsync(currentPage, currentSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReviewById(int id)
        {
            var Review = await _reviewRepo.GetReviewsAsync(id);
            return Review == null ? NotFound() : Ok(Review);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddNewReview([FromBody] UpdateReviewDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var model = new ReviewModel
            {
                Rating = dto.Rating,
                Content = dto.Content,
                ReviewDate = dto.ReviewDate,
                UserId = dto.UserId,
                OrderDetailId = dto.OrderDetailId
            };

            var newReviewId = await _reviewRepo.AddReviewAsync(model);
            var Review = await _reviewRepo.GetReviewsAsync(newReviewId);
            return Review == null ? NotFound() : Ok(Review);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateReview(int id, [FromBody] UpdateReviewDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var existingReview = await _reviewRepo.GetReviewsAsync(id);
            if (existingReview == null)
            {
                return NotFound($"Không tìm thấy đánh giá có ID = {id}");
            }

            existingReview.Rating = dto.Rating;
            existingReview.Content = dto.Content;
            existingReview.ReviewDate = dto.ReviewDate;
            existingReview.UserId = dto.UserId;
            existingReview.OrderDetailId = dto.OrderDetailId;

            await _reviewRepo.UpdateReviewAsync(id, existingReview);
            return Ok(existingReview);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteReview([FromRoute] int id)
        {
            var message = await _reviewRepo.DeleteReviewAsync(id);
            return Ok(new { message });
        }

        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetReviewsByProductId(int productId)
        {
            var reviews = await _reviewRepo.GetReviewsByProductIdAsync(productId);

            if (reviews == null || !reviews.Any())
            {
                return NotFound($"Không có đánh giá nào cho sản phẩm có ID = {productId}");
            }

            return Ok(reviews);
        }

    }
}
