using SWD392.DTOs.Pagination;
using SWD392.Models;

namespace SWD392.Repositories
{
    public interface IUnitRepository
    {
        Task<PagedResult<UnitModel>> GetAllUnitsAsync(int pageNumber, int pageSize);
        public Task<UnitModel> GetUnitsAsync(int id);

        public Task<int> AddUnitAsync(UnitModel model);

        public Task UpdateUnitAsync(int id, UnitModel model);
        public Task<string> DeleteUnitAsync(int id);
    }
}
