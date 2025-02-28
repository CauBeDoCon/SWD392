using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWD392.DB;
using SWD392.DTOs.Pagination;
using SWD392.Models;
using SWD392.Repositories;

namespace SWD392.Repositories
{
    public class BlogRepository : IBlogRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public BlogRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<int> AddBlogAsync(BlogModel model)
        {
            var newBlog = _mapper.Map<Blog>(model);
            _context.Blogs!.Add(newBlog);
            await _context.SaveChangesAsync();
            return newBlog.Id;
        }

        public async Task<string> DeleteBlogAsync(int id)
        {
            var deleteBlog = await _context.Blogs!.FindAsync(id);

            if (deleteBlog == null)
            {
                throw new KeyNotFoundException($"Blog với ID {id} không tìm thấy.");
            }

            _context.Blogs.Remove(deleteBlog);
            await _context.SaveChangesAsync();

            return $"Blog với ID {id} đã xoá thành công.";
        }

        public async Task<PagedResult<BlogModel>> GetAllBlogsAsync(int pageNumber, int pageSize)
        {
            int totalCount = await _context.Blogs!.CountAsync();

            var Blogs = await _context.Blogs!
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var mappedData = _mapper.Map<List<BlogModel>>(Blogs);

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
            var Blogs = await _context.Blogs.FindAsync(id);
            return _mapper.Map<BlogModel>(Blogs);
        }

        public async Task UpdateBlogAsync(int id, BlogModel model)
        {
            if (id != model.Id)
            {
                throw new ArgumentException("ID không khớp giữa request và model.");
            }

            var existingEntity = await _context.Blogs!.FindAsync(id);
            if (existingEntity == null)
            {
                throw new KeyNotFoundException($"Blog với ID {id} không tìm thấy.");
            }

            _context.Entry(existingEntity).State = EntityState.Detached;

            var updateBlog = _mapper.Map<Blog>(model);

            _context.Blogs.Attach(updateBlog);
            _context.Entry(updateBlog).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }
    }
}
