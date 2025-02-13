using SWD392.Models;

namespace SWD392.Repositories
{
    public interface IBrandOriginRepository
    {
        public Task<List<BrandOriginModel>> GetAllBrandOriginsAsync();
        public Task<BrandOriginModel> GetBrandOriginsAsync(int id);

        public Task<int> AddBrandOriginAsync(BrandOriginModel model);

        public Task UpdateBrandOriginAsync(int id, BrandOriginModel model);
        public Task DeleteBrandOriginAsync(int id);
    }
}
