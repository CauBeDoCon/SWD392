using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SWD392.DB;
using SWD392.DTOs;
using SWD392.DTOs.Pagination;
using SWD392.Models;
using System.Security.Claims;

namespace SWD392.Repositories
{
    public class BlogRepository : IBlogRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BlogRepository(ApplicationDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        private string GetUserId()
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException("Không thể xác định UserId từ JWT.");
            }
            return userId;
        }

        private void EnsureStaffRole()
        {
            if (!_httpContextAccessor.HttpContext.User.IsInRole("Staff"))
            {
                throw new UnauthorizedAccessException("Bạn không có quyền thực hiện thao tác này.");
            }
        }

        public async Task<int> AddBlogAsync(BlogDTO dto)
        {
            EnsureStaffRole();

            string userId = GetUserId();

            var newBlog = new Blog
            {
                Title = dto.Title,
                Content = dto.Content,
                Tags = dto.Tags,
                Image = dto.Image,
                UserId = userId,
                PublishDate = DateTime.UtcNow
            };

            _context.Blogs.Add(newBlog);
            await _context.SaveChangesAsync();
            return newBlog.Id;
        }

        public async Task<string> DeleteBlogAsync(int id)
        {
            EnsureStaffRole();

            var blogToDelete = await _context.Blogs.FindAsync(id);

            if (blogToDelete == null)
            {
                throw new KeyNotFoundException($"Blog với ID {id} không tìm thấy.");
            }

            _context.Blogs.Remove(blogToDelete);
            await _context.SaveChangesAsync();

            return $"Blog với ID {id} đã xoá thành công.";
        }

        public async Task<PagedResult<BlogModel>> GetAllBlogsAsync(int pageNumber, int pageSize)
        {
            int totalCount = await _context.Blogs.CountAsync();

            var blogs = await _context.Blogs
                .OrderByDescending(b => b.PublishDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var mappedData = _mapper.Map<List<BlogModel>>(blogs);

            return new PagedResult<BlogModel>
            {
                Items = mappedData,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<BlogModel> GetBlogsAsync(int id)
        {
            var blog = await _context.Blogs.FindAsync(id);
            return _mapper.Map<BlogModel>(blog);
        }

        public async Task UpdateBlogAsync(int id, UpdateBlogDto dto)
        {
            EnsureStaffRole();

            var existingEntity = await _context.Blogs.FindAsync(id);

            if (existingEntity == null)
            {
                throw new KeyNotFoundException($"Blog với ID {id} không tìm thấy.");
            }

            existingEntity.Title = dto.Title;
            existingEntity.Content = dto.Content;
            existingEntity.Tags = dto.Tags;
            existingEntity.Image = dto.Image;

            await _context.SaveChangesAsync();
        }
    }
}
