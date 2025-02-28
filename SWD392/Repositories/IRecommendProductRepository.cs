using SWD392.DTOs.Pagination;
using SWD392.Models;

namespace SWD392.Repositories
{
    public interface IRecommendProductRepository
    {
        Task<PagedResult<RecommendProductModel>> GetAllRecommendProductsAsync(int pageNumber, int pageSize);
        public Task<RecommendProductModel> GetRecommendProductsAsync(int id);

        public Task<int> AddRecommendProductAsync(RecommendProductModel model);

        public Task UpdateRecommendProductAsync(int id, RecommendProductModel model);
        public Task<string> DeleteRecommendProductAsync(int id);
    }
}
