using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWD392.DB;
using SWD392.DTOs.Pagination;
using SWD392.Models;
using SWD392.Repositories;

namespace SWD392.Repositories
{
    public class BookingResultRepository : IBookingResultRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public BookingResultRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<int> AddBookingResultAsync(BookingResultModel model)
        {
            var newBookingResult = _mapper.Map<BookingResult>(model);
            _context.BookingResults!.Add(newBookingResult);
            await _context.SaveChangesAsync();
            return newBookingResult.Id;
        }

        public async Task<string> DeleteBookingResultAsync(int id)
        {
            var deleteBookingResult = await _context.BookingResults!.FindAsync(id);

            if (deleteBookingResult == null)
            {
                throw new KeyNotFoundException($"Kết quả đặt lịch với ID {id} không tìm thấy.");
            }

            _context.BookingResults.Remove(deleteBookingResult);
            await _context.SaveChangesAsync();

            return $"Kết quả đặt lịch với ID {id} đã xoá thành công.";
        }

        public async Task<PagedResult<BookingResultModel>> GetAllBookingResultsAsync(int pageNumber, int pageSize)
        {
            int totalCount = await _context.BookingResults!.CountAsync();

            var BookingResults = await _context.BookingResults!
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var mappedData = _mapper.Map<List<BookingResultModel>>(BookingResults);

            return new PagedResult<BookingResultModel>
            {
                Items = mappedData,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<BookingResultModel> GetBookingResultsAsync(int id)
        {
            var BookingResults = await _context.BookingResults.FindAsync(id);
            return _mapper.Map<BookingResultModel>(BookingResults);
        }

        public async Task UpdateBookingResultAsync(int id, BookingResultModel model)
        {
            if (id != model.Id)
            {
                throw new ArgumentException("ID không khớp giữa request và model.");
            }

            var existingEntity = await _context.BookingResults!.FindAsync(id);
            if (existingEntity == null)
            {
                throw new KeyNotFoundException($"Kết quả đặt lịch với ID {id} không tìm thấy.");
            }

            _context.Entry(existingEntity).State = EntityState.Detached;

            var updateBookingResult = _mapper.Map<BookingResult>(model);

            _context.BookingResults.Attach(updateBookingResult);
            _context.Entry(updateBookingResult).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }
    }
}
