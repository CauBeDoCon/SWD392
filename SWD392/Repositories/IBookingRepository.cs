using SWD392.DTOs.Pagination;
using SWD392.Models;

namespace SWD392.Repositories
{
    public interface IBookingRepository
    {
        Task<PagedResult<BookingModel>> GetAllBookingsAsync(int pageNumber, int pageSize);
        public Task<BookingModel> GetBookingsAsync(int id);

        public Task<int> AddBookingAsync(BookingModel model);

        public Task UpdateBookingAsync(int id, BookingModel model);
        public Task<string> DeleteBookingAsync(int id);
    }
}
