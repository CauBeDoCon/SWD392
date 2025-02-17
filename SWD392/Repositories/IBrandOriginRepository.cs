using SWD392.DTOs.Pagination;
using SWD392.Models;

namespace SWD392.Repositories
{
    public interface IBrandOriginRepository
    {
        Task<PagedResult<BrandOriginModel>> GetAllBrandOriginsAsync(int pageNumber, int pageSize);

        public Task<BrandOriginModel> GetBrandOriginsAsync(int id);

        public Task<int> AddBrandOriginAsync(BrandOriginModel model);

        public Task UpdateBrandOriginAsync(int id, BrandOriginModel model);
        public Task<string> DeleteBrandOriginAsync(int id);
    }
}
