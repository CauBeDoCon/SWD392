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
            try
            {
                var comment = await _commentRepository.GetCommentByIdAsync(id);
                if (comment == null)
                    return NotFound(new { message = "Comment không tồn tại." });
                return Ok(comment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi lấy comment.", error = ex.Message });
            }
        }

        [HttpGet("review/{reviewId}")]
        public async Task<IActionResult> GetCommentByReviewId(int reviewId)
        {
            try
            {
                var comment = await _commentRepository.GetCommentByReviewIdAsync(reviewId);
                if (comment == null)
                    return NotFound(new { message = "Comment cho review này không tồn tại." });
                return Ok(comment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi lấy comment.", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] CommentDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(new { message = "Không tìm thấy thông tin user." });

                var commentId = await _commentRepository.CreateCommentAsync(dto, userId);
                return CreatedAtAction(nameof(GetCommentById), new { id = commentId }, new { id = commentId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi tạo comment.", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment(int id, [FromBody] UpdateCommentDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(new { message = "Không tìm thấy thông tin user." });

                await _commentRepository.UpdateCommentAsync(id, dto, userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi cập nhật comment.", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(new { message = "Không tìm thấy thông tin user." });

                await _commentRepository.DeleteCommentAsync(id, userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi xóa comment.", error = ex.Message });
            }
        }
    }
}
