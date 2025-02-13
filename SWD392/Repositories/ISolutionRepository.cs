using SWD392.Models;

namespace SWD392.Repositories
{
    public interface ISolutionRepository
    {
        public Task<List<SolutionModel>> GetAllSolutionsAsync();
        public Task<SolutionModel> GetSolutionsAsync(int id);

        public Task<int> AddSolutionAsync(SolutionModel model);

        public Task UpdateSolutionAsync(int id, SolutionModel model);
        public Task DeleteSolutionAsync(int id);
    }
}
