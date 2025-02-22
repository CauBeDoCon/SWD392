using SWD392.DTOs.Pagination;
using SWD392.Models;

namespace SWD392.Repositories
{
    public interface IReviewRepository
    {
        Task<PagedResult<ReviewModel>> GetAllReviewsAsync(int pageNumber, int pageSize);
        public Task<ReviewModel> GetReviewsAsync(int id);

        public Task<int> AddReviewAsync(ReviewModel model);

        public Task UpdateReviewAsync(int id, ReviewModel model);
        public Task<string> DeleteReviewAsync(int id);
    }
}
