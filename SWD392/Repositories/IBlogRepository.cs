using SWD392.DTOs.Pagination;
using SWD392.Models;

namespace SWD392.Repositories
{
    public interface IBlogRepository
    {
        Task<PagedResult<BlogModel>> GetAllBlogsAsync(int pageNumber, int pageSize);
        public Task<BlogModel> GetBlogsAsync(int id);

        public Task<int> AddBlogAsync(BlogModel model);

        public Task UpdateBlogAsync(int id, BlogModel model);
        public Task<string> DeleteBlogAsync(int id);
    }
}
