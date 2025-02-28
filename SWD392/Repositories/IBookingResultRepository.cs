using SWD392.DTOs.Pagination;
using SWD392.Models;

namespace SWD392.Repositories
{
    public interface IBookingResultRepository
    {
        Task<PagedResult<BookingResultModel>> GetAllBookingResultsAsync(int pageNumber, int pageSize);
        public Task<BookingResultModel> GetBookingResultsAsync(int id);

        public Task<int> AddBookingResultAsync(BookingResultModel model);

        public Task UpdateBookingResultAsync(int id, BookingResultModel model);
        public Task<string> DeleteBookingResultAsync(int id);
    }
}
