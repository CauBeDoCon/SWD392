using SWD392.DTOs.Pagination;
using SWD392.Models;

namespace SWD392.Repositories
{
    public interface IRoutineRepository
    {
        Task<PagedResult<RoutineModel>> GetAllRoutinesAsync(int pageNumber, int pageSize);
        public Task<RoutineModel> GetRoutinesAsync(int id);

        public Task<int> AddRoutineAsync(RoutineModel model);

        public Task UpdateRoutineAsync(int id, RoutineModel model);
        public Task<string> DeleteRoutineAsync(int id);
    }
}
