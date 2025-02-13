using SWD392.Models;

namespace SWD392.Repositories
{
    public interface IBrandRepository
    {
        public Task<List<BrandModel>> GetAllBrandsAsync();
        public Task<BrandModel> GetBrandsAsync(int id);

        public Task<int> AddBrandAsync(BrandModel model);

        public Task UpdateBrandAsync(int id, BrandModel model);
        public Task DeleteBrandAsync(int id);
    }
}
