using SWD392.DTOs;
using SWD392.Model;
using SWD392.Models;
using System.Threading.Tasks;

namespace SWD392.Repositories
{
    public interface ICommentRepository
    {
        Task<ResponseMessage<CommentModel>> GetCommentByIdAsync(int commentId);
        Task<ResponseMessage<CommentModel>> GetCommentByReviewIdAsync(int reviewId);
        Task<ResponseMessage<int>> CreateCommentAsync(CommentDTO dto, string currentUserId);
        Task<ResponseMessage<bool>> UpdateCommentAsync(int commentId, UpdateCommentDTO dto, string currentUserId);
        Task<ResponseMessage<bool>> DeleteCommentAsync(int commentId, string currentUserId);
    }

}
