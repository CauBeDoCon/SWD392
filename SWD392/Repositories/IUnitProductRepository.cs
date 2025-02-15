using SWD392.DTOs.Pagination;
using SWD392.Models;

namespace SWD392.Repositories
{
    public interface IUnitProductRepository
    {
        Task<PagedResult<UnitProductModel>> GetAllUnitProductsAsync(int pageNumber, int pageSize);
        public Task<UnitProductModel> GetUnitProductsAsync(int id);

        public Task<int> AddUnitProductAsync(UnitProductModel model);

        public Task UpdateUnitProductAsync(int id, UnitProductModel model);
        public Task<string> DeleteUnitProductAsync(int id);
    }
}
