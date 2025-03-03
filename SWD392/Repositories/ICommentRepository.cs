using SWD392.DTOs;
using SWD392.Model;
using SWD392.Models;
using System.Threading.Tasks;

namespace SWD392.Repositories
{
    public interface ICommentRepository
    {
        Task<CommentModel> GetCommentByIdAsync(int commentId);

        Task<CommentModel> GetCommentByReviewIdAsync(int reviewId);

        Task<int> CreateCommentAsync(CommentDTO dto, string currentUserId);
        Task UpdateCommentAsync(int commentId, UpdateCommentDTO dto, string currentUserId);
        Task DeleteCommentAsync(int commentId, string currentUserId);
    }
}
