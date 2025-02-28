using SWD392.DTOs.Pagination;
using SWD392.DTOs;
using SWD392.Models;

namespace SWD392.Repositories
{
    public interface IBlogRepository
    {
        Task<PagedResult<BlogModel>> GetAllBlogsAsync(int pageNumber, int pageSize);
        Task<BlogModel> GetBlogsAsync(int id);
        Task<int> AddBlogAsync(BlogDTO dto);
        Task UpdateBlogAsync(int id, UpdateBlogDto dto);
        Task<string> DeleteBlogAsync(int id);
    }
}
