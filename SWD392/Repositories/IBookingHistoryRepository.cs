using SWD392.DTOs.Pagination;
using SWD392.Models;

namespace SWD392.Repositories
{
    public interface IBookingHistoryRepository
    {
        Task<PagedResult<BookingHistoryModel>> GetAllBookingHistoriesAsync(int pageNumber, int pageSize);
        public Task<BookingHistoryModel> GetBookingHistoriesAsync(int id);

        public Task<int> AddBookingHistoryAsync(BookingHistoryModel model);

        public Task UpdateBookingHistoryAsync(int id, BookingHistoryModel model);
        public Task<string> DeleteBookingHistoryAsync(int id);
    }
}
