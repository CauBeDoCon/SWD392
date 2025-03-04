using SWD392.DTOs;
using SWD392.Models;

namespace SWD392.Repositories
{
    public interface IReviewRepository
    {
        Task<ReviewModel> GetReviewById(int reviewId);

        Task<IEnumerable<ReviewModel>> GetReviewsByProduct(int productId);

        Task<int> CreateReviewAsync(ReviewDTO dto, string currentUserId);

        Task UpdateReviewAsync(int reviewId, UpdateReviewDTO dto, string currentUserId);

        Task DeleteReviewAsync(int reviewId, string currentUserId);
    }
}
