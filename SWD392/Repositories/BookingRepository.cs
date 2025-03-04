using Microsoft.EntityFrameworkCore;
using SWD392.DB;
using SWD392.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWD392.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly ApplicationDbContext _context;

        public BookingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<BookingDTO>> GetAvailableBookingsAsync(string doctorId)
        {
            return await _context.Bookings
                .Where(b => b.DoctorId == doctorId && b.Status == "Available")
                .OrderBy(b => b.TimeSlot)
                .Select(b => new BookingDTO
                {
                    BookingId = b.BookingId,
                    TimeSlot = b.TimeSlot,
                    Status = b.Status
                })
                .ToListAsync();
        }

       
        public async Task<bool> BookAppointmentAsync(BookingRequestDTO request, string customerUsername)
        {
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.BookingId == request.BookingId && b.Status == "Available");

            if (booking == null)
            {
                return false;
            }

            booking.CustomerUsername = customerUsername;
            booking.Status = "Booked";

            await _context.SaveChangesAsync();
            return true;
        }

    
        public async Task<List<BookingDTO>> GetDoctorBookingsAsync(string doctorId)
        {
            return await _context.Bookings
                .Where(b => b.DoctorId == doctorId)
                .OrderBy(b => b.TimeSlot)
                .Select(b => new BookingDTO
                {
                    BookingId = b.BookingId,
                    TimeSlot = b.TimeSlot,
                    Status = b.Status,
                    CustomerUsername = b.CustomerUsername 
                })
                .ToListAsync();
        }


        public async Task CreateDoctorBookingsAsync(string doctorId)
        {
            DateTime today = DateTime.Today;

            List<DateTime> timeSlots = new List<DateTime>
            {
                today.AddHours(8), today.AddHours(9),
                today.AddHours(10), today.AddHours(11),
                today.AddHours(13), today.AddHours(14),
                today.AddHours(15), today.AddHours(16)
            };

            List<Booking> bookings = new List<Booking>();

            foreach (var slot in timeSlots)
            {
                bookings.Add(new Booking
                {
                    DoctorId = doctorId,
                    TimeSlot = slot,
                    Status = "Available"
                });
            }

            _context.Bookings.AddRange(bookings);
            await _context.SaveChangesAsync();
        }
    }
}
