using SWD392.DTOs.Pagination;
using SWD392.Models;

namespace SWD392.Repositories
{
    public interface ISolutionRepository
    {
        Task<PagedResult<SolutionModel>> GetAllSolutionsAsync(int pageNumber, int pageSize);
        public Task<SolutionModel> GetSolutionsAsync(int id);

        public Task<int> AddSolutionAsync(SolutionModel model);

        public Task UpdateSolutionAsync(int id, SolutionModel model);
        public Task<string> DeleteSolutionAsync(int id);
    }
}
