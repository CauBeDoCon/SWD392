using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWD392.DTOs;
using SWD392.Repositories;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SWD392.Controllers
{
    [Route("api/comments")]
    [ApiController]
    [Authorize(Roles = "Doctor")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;

        public CommentController(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCommentById(int id)
        {
            var response = await _commentRepository.GetCommentByIdAsync(id);
            if (!response.Success)
                return NotFound(new { message = response.Message });
            return Ok(response.Data);
        }

        [HttpGet("review/{reviewId}")]
        public async Task<IActionResult> GetCommentByReviewId(int reviewId)
        {
            var response = await _commentRepository.GetCommentByReviewIdAsync(reviewId);
            if (!response.Success)
                return NotFound(new { message = response.Message });
            return Ok(response.Data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] CommentDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "Không tìm thấy thông tin user." });

            var response = await _commentRepository.CreateCommentAsync(dto, userId);
            if (!response.Success)
                return BadRequest(new { message = response.Message });

            return CreatedAtAction(nameof(GetCommentById), new { id = response.Data }, new { id = response.Data });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment(int id, [FromBody] UpdateCommentDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "Không tìm thấy thông tin user." });

            var response = await _commentRepository.UpdateCommentAsync(id, dto, userId);
            if (!response.Success)
                return BadRequest(new { message = response.Message });

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "Không tìm thấy thông tin user." });

            var response = await _commentRepository.DeleteCommentAsync(id, userId);
            if (!response.Success)
                return BadRequest(new { message = response.Message });

            return NoContent();
        }
    }
}
