using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWD392.DTOs;
using SWD392.Helpers;
using SWD392.Models;
using SWD392.Repositories;
using System.Security.Claims;

namespace SWD392.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private readonly IBlogRepository _blogRepo;

        public BlogsController(IBlogRepository repo)
        {
            _blogRepo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBlogs([FromQuery] int? pageNumber, [FromQuery] int? pageSize)
        {
            int currentPage = pageNumber ?? 1;
            int currentSize = pageSize ?? 10;

            var result = await _blogRepo.GetAllBlogsAsync(currentPage, currentSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBlogById(int id)
        {
            var blog = await _blogRepo.GetBlogsAsync(id);
            if (blog == null)
            {
                return NotFound($"Không tìm thấy blog có ID = {id}");
            }
            return Ok(blog);
        }

        [HttpPost]
        [Authorize(Roles = AppRole.Staff)]
        public async Task<IActionResult> AddNewBlog([FromBody] BlogDTO dto)
        {
            if (dto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Không xác định được người dùng.");
            }

            var newBlogId = await _blogRepo.AddBlogAsync(dto);
            var blog = await _blogRepo.GetBlogsAsync(newBlogId);
            return blog == null ? NotFound() : Ok(blog);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = AppRole.Staff)]
        public async Task<IActionResult> UpdateBlog(int id, [FromBody] UpdateBlogDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var existingBlog = await _blogRepo.GetBlogsAsync(id);
            if (existingBlog == null)
            {
                return NotFound($"Không tìm thấy blog có ID = {id}");
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Không xác định được người dùng.");
            }

            if (existingBlog.UserId != userId)
            {
                return Forbid("Bạn không có quyền cập nhật blog này.");
            }

            await _blogRepo.UpdateBlogAsync(id, dto);

            var updatedBlog = await _blogRepo.GetBlogsAsync(id);
            return Ok(updatedBlog);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = AppRole.Staff)]
        public async Task<IActionResult> DeleteBlog([FromRoute] int id)
        {
            var existingBlog = await _blogRepo.GetBlogsAsync(id);
            if (existingBlog == null)
            {
                return NotFound($"Không tìm thấy blog có ID = {id}");
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Không xác định được người dùng.");
            }

            if (existingBlog.UserId != userId)
            {
                return Forbid("Bạn không có quyền xóa blog này.");
            }

            var message = await _blogRepo.DeleteBlogAsync(id);
            return Ok(new { message });
        }
    }
}
