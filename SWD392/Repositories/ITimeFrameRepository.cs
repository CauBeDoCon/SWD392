using SWD392.DTOs.Pagination;
using SWD392.Models;

namespace SWD392.Repositories
{
    public interface ITimeFrameRepository
    {
        Task<PagedResult<TimeFrameModel>> GetAllTimeFramesAsync(int pageNumber, int pageSize);
        public Task<TimeFrameModel> GetTimeFramesAsync(int id);

        public Task<int> AddTimeFrameAsync(TimeFrameModel model);

        public Task UpdateTimeFrameAsync(int id, TimeFrameModel model);
        public Task<string> DeleteTimeFrameAsync(int id);
    }
}
