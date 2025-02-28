using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWD392.DB;
using SWD392.DTOs.Pagination;
using SWD392.Models;
using SWD392.Repositories;

namespace SWD392.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public BookingRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<int> AddBookingAsync(BookingModel model)
        {
            var newBooking = _mapper.Map<Booking>(model);
            _context.Bookings!.Add(newBooking);
            await _context.SaveChangesAsync();
            return newBooking.Id;
        }

        public async Task<string> DeleteBookingAsync(int id)
        {
            var deleteBooking = await _context.Bookings!.FindAsync(id);

            if (deleteBooking == null)
            {
                throw new KeyNotFoundException($"Đặt lịch với ID {id} không tìm thấy.");
            }

            _context.Bookings.Remove(deleteBooking);
            await _context.SaveChangesAsync();

            return $"Đặt lịch với ID {id} đã xoá thành công.";
        }

        public async Task<PagedResult<BookingModel>> GetAllBookingsAsync(int pageNumber, int pageSize)
        {
            int totalCount = await _context.Bookings!.CountAsync();

            var Bookings = await _context.Bookings!
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var mappedData = _mapper.Map<List<BookingModel>>(Bookings);

            return new PagedResult<BookingModel>
            {
                Items = mappedData,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<BookingModel> GetBookingsAsync(int id)
        {
            var Bookings = await _context.Bookings.FindAsync(id);
            return _mapper.Map<BookingModel>(Bookings);
        }

        public async Task UpdateBookingAsync(int id, BookingModel model)
        {
            if (id != model.Id)
            {
                throw new ArgumentException("ID không khớp giữa request và model.");
            }

            var existingEntity = await _context.Bookings!.FindAsync(id);
            if (existingEntity == null)
            {
                throw new KeyNotFoundException($"Đặt lịch với ID {id} không tìm thấy.");
            }

            _context.Entry(existingEntity).State = EntityState.Detached;

            var updateBooking = _mapper.Map<Booking>(model);

            _context.Bookings.Attach(updateBooking);
            _context.Entry(updateBooking).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }
    }
}
