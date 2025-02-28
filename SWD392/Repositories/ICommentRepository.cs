using SWD392.DTOs.Pagination;
using SWD392.Models;

namespace SWD392.Repositories
{
    public interface ICommentRepository
    {
        Task<PagedResult<CommentModel>> GetAllCommentsAsync(int pageNumber, int pageSize);
        public Task<CommentModel> GetCommentsAsync(int id);

        public Task<int> AddCommentAsync(CommentModel model);

        public Task UpdateCommentAsync(int id, CommentModel model);
        public Task<string> DeleteCommentAsync(int id);
    }
}
