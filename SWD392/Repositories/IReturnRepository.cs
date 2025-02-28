using SWD392.DTOs.Pagination;
using SWD392.Models;

namespace SWD392.Repositories
{
    public interface IReturnRepository
    {
        Task<PagedResult<ReturnModel>> GetAllReturnsAsync(int pageNumber, int pageSize);
        public Task<ReturnModel> GetReturnsAsync(int id);

        public Task<int> AddReturnAsync(ReturnModel model);

        public Task UpdateReturnAsync(int id, ReturnModel model);
        public Task<string> DeleteReturnAsync(int id);
    }
}
