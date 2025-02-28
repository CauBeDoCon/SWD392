using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWD392.DB;
using SWD392.DTOs.Pagination;
using SWD392.Models;
using SWD392.Repositories;

namespace SWD392.Repositories
{
    public class BookingHistoryRepository : IBookingHistoryRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public BookingHistoryRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<int> AddBookingHistoryAsync(BookingHistoryModel model)
        {
            var newBookingHistory = _mapper.Map<BookingHistory>(model);
            _context.BookingHistories!.Add(newBookingHistory);
            await _context.SaveChangesAsync();
            return newBookingHistory.Id;
        }

        public async Task<string> DeleteBookingHistoryAsync(int id)
        {
            var deleteBookingHistory = await _context.BookingHistories!.FindAsync(id);

            if (deleteBookingHistory == null)
            {
                throw new KeyNotFoundException($"Lịch sử đặt lịch với ID {id} không tìm thấy.");
            }

            _context.BookingHistories.Remove(deleteBookingHistory);
            await _context.SaveChangesAsync();

            return $"Lịch sử đặt lịch với ID {id} đã xoá thành công.";
        }

        public async Task<PagedResult<BookingHistoryModel>> GetAllBookingHistoriesAsync(int pageNumber, int pageSize)
        {
            int totalCount = await _context.BookingHistories!.CountAsync();

            var BookingHistorys = await _context.BookingHistories!
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var mappedData = _mapper.Map<List<BookingHistoryModel>>(BookingHistorys);

            return new PagedResult<BookingHistoryModel>
            {
                Items = mappedData,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<BookingHistoryModel> GetBookingHistoriesAsync(int id)
        {
            var BookingHistorys = await _context.BookingHistories.FindAsync(id);
            return _mapper.Map<BookingHistoryModel>(BookingHistorys);
        }

        public async Task UpdateBookingHistoryAsync(int id, BookingHistoryModel model)
        {
            if (id != model.Id)
            {
                throw new ArgumentException("ID không khớp giữa request và model.");
            }

            var existingEntity = await _context.BookingHistories!.FindAsync(id);
            if (existingEntity == null)
            {
                throw new KeyNotFoundException($"Lịch sử đặt lịch với ID {id} không tìm thấy.");
            }

            _context.Entry(existingEntity).State = EntityState.Detached;

            var updateBookingHistory = _mapper.Map<BookingHistory>(model);

            _context.BookingHistories.Attach(updateBookingHistory);
            _context.Entry(updateBookingHistory).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }
    }
}
