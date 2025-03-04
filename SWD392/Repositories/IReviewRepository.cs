using SWD392.DTOs;
using SWD392.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWD392.Repositories
{
    public interface IReviewRepository
    {
        Task<ResponseMessage<ReviewModel>> GetReviewById(int reviewId);

        Task<ResponseMessage<IEnumerable<ReviewModel>>> GetReviewsByProduct(int productId);

        Task<ResponseMessage<int>> CreateReviewAsync(int orderDetailId, ReviewDTO dto, string currentUserId);

        Task<ResponseMessage<bool>> UpdateReviewAsync(int reviewId, UpdateReviewDTO dto, string currentUserId);

        Task<ResponseMessage<bool>> DeleteReviewAsync(int reviewId, string currentUserId);
    }
}
