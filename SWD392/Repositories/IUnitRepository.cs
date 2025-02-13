using SWD392.Models;

namespace SWD392.Repositories
{
    public interface IUnitRepository
    {
        public Task<List<UnitModel>> GetAllUnitsAsync();
        public Task<UnitModel> GetUnitsAsync(int id);

        public Task<int> AddUnitAsync(UnitModel model);

        public Task UpdateUnitAsync(int id, UnitModel model);
        public Task DeleteUnitAsync(int id);
    }
}
