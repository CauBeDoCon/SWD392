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
    public class CommentsController : ControllerBase
    {
        private readonly ICommentRepository _commentRepo;

        public CommentsController(ICommentRepository repo)
        {
            _commentRepo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllComments([FromQuery] int? pageNumber, [FromQuery] int? pageSize)
        {
            int currentPage = pageNumber ?? 1;
            int currentSize = pageSize ?? 10;

            var result = await _commentRepo.GetAllCommentsAsync(currentPage, currentSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCommentById(int id)
        {
            var Comment = await _commentRepo.GetCommentsAsync(id);
            return Comment == null ? NotFound() : Ok(Comment);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddNewComment([FromBody] UpdateCommentDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var model = new CommentModel
            {
                Content = dto.Content,
                CommentDate = dto.CommentDate,
                UserId = dto.UserId,
                ReviewId = dto.ReviewId
            };

            var newCommentId = await _commentRepo.AddCommentAsync(model);
            var Comment = await _commentRepo.GetCommentsAsync(newCommentId);
            return Comment == null ? NotFound() : Ok(Comment);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateComment(int id, [FromBody] UpdateCommentDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var existingComment = await _commentRepo.GetCommentsAsync(id);
            if (existingComment == null)
            {
                return NotFound($"Không tìm thấy phản hồi có ID = {id}");
            }

            existingComment.Content = dto.Content;
            existingComment.CommentDate = dto.CommentDate;
            existingComment.UserId = dto.UserId;
            existingComment.ReviewId = dto.ReviewId;

            await _commentRepo.UpdateCommentAsync(id, existingComment);
            return Ok(existingComment);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteComment([FromRoute] int id)
        {
            var message = await _commentRepo.DeleteCommentAsync(id);
            return Ok(new { message });
        }
    }
}
