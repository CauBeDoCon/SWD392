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
            var Blog = await _blogRepo.GetBlogsAsync(id);
            return Blog == null ? NotFound() : Ok(Blog);
        }

        [HttpPost]
        [Authorize(Roles = AppRole.Staff)] // Chỉ Staff mới có quyền
        public async Task<IActionResult> AddNewBlog([FromBody] UpdateBlogDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            // Lấy UserId từ JWT Claims
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Không xác định được người dùng.");
            }

            var model = new BlogModel
            {
                Title = dto.Title,
                Content = dto.Content,
                PublishDate = dto.PublishDate,
                Tags = dto.Tags,
                Image = dto.Image,
                UserId = userId // Gán userId từ token
            };

            var newBlogId = await _blogRepo.AddBlogAsync(model);
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

            // Lấy UserId từ JWT Claims
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Không xác định được người dùng.");
            }

            // Chỉ cho phép cập nhật nếu user là người tạo blog
            if (existingBlog.UserId != userId)
            {
                return Forbid("Bạn không có quyền cập nhật blog này.");
            }

            existingBlog.Title = dto.Title;
            existingBlog.Content = dto.Content;
            existingBlog.PublishDate = dto.PublishDate;
            existingBlog.Tags = dto.Tags;
            existingBlog.Image = dto.Image;

            await _blogRepo.UpdateBlogAsync(id, existingBlog);
            return Ok(existingBlog);
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

            // Lấy UserId từ JWT Claims
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Không xác định được người dùng.");
            }

            // Chỉ cho phép xóa nếu user là người tạo blog
            if (existingBlog.UserId != userId)
            {
                return Forbid("Bạn không có quyền xóa blog này.");
            }

            var message = await _blogRepo.DeleteBlogAsync(id);
            return Ok(new { message });
        }

    }
}
