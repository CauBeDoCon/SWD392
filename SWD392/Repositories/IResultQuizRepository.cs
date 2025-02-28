using SWD392.DTOs.Pagination;
using SWD392.Models;

namespace SWD392.Repositories
{
    public interface IResultQuizRepository
    {
        Task<PagedResult<ResultQuizModel>> GetAllResultQuizzesAsync(int pageNumber, int pageSize);
        public Task<ResultQuizModel> GetResultQuizzesAsync(int id);

        public Task<int> AddResultQuizAsync(ResultQuizModel model);

        public Task UpdateResultQuizAsync(int id, ResultQuizModel model);
        public Task<string> DeleteResultQuizAsync(int id);
    }
}
