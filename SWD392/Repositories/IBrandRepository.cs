using SWD392.DTOs.Pagination;
using SWD392.Models;

namespace SWD392.Repositories
{
    public interface IBrandRepository
    {
        Task<PagedResult<BrandModel>> GetAllBrandsAsync(int pageNumber, int pageSize);
        public Task<BrandModel> GetBrandsAsync(int id);

        public Task<int> AddBrandAsync(BrandModel model);

        public Task UpdateBrandAsync(int id, BrandModel model);
        public Task<string> DeleteBrandAsync(int id);
    }
}
