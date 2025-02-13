using SWD392.Models;

namespace SWD392.Repositories
{
    public interface IUnitProductRepository
    {
        public Task<List<UnitProductModel>> GetAllUnitProductsAsync();
        public Task<UnitProductModel> GetUnitProductsAsync(int id);

        public Task<int> AddUnitProductAsync(UnitProductModel model);

        public Task UpdateUnitProductAsync(int id, UnitProductModel model);
        public Task DeleteUnitProductAsync(int id);
    }
}
