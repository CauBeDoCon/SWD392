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

       
        public async Task<bool> RequestAppointmentAsync(BookingRequestDTO request, string CustomerId)
        {
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.BookingId == request.BookingId && b.Status == "Available");

            if (booking == null)
            {
                return false;
            }

            booking.CustomerId = CustomerId;
            booking.Status = "Pending"; 

            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<List<BookingDTO>> GetCustomerAppointmentsAsync(string CustomerId)
        {
            return await _context.Bookings
                .Where(b => b.CustomerId == CustomerId)
                .OrderBy(b => b.TimeSlot)
                .Join(_context.Users,
                      booking => booking.DoctorId, 
                      user => user.Id,
                      (booking, user) => new BookingDTO
                      {
                          BookingId = booking.BookingId,
                          TimeSlot = booking.TimeSlot,
                          Status = booking.Status,
                          CustomerId = booking.CustomerId,
                          DoctorAvatar = user.Avatar,
                          DoctorFirstName = user.FirstName, 
                          DoctorLastName = user.LastName
                      })
                .ToListAsync();
        }




        public async Task<bool> CancelAppointmentAsync(int bookingId, string CustomerId)
        {
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.BookingId == bookingId && b.CustomerId == CustomerId);

            if (booking == null)
            {
                return false; 
            }

           
            if (booking.Status == "Confirmed")
            {
                return false; 
            }

        
            booking.Status = "Cancelled";
            await _context.SaveChangesAsync();
            return true;
        }



        public async Task<List<BookingDTO>> GetDoctorScheduleAsync(string doctorId)
        {
            return await _context.Bookings
                .Where(b => b.DoctorId == doctorId && b.Status == "Confirmed")
                .OrderBy(b => b.TimeSlot)
                .Select(b => new BookingDTO
                {
                    BookingId = b.BookingId,
                    TimeSlot = b.TimeSlot,
                    Status = b.Status,
                    CustomerId = b.CustomerId
                })
                .ToListAsync();
        }

     
        public async Task<bool> CompleteAppointmentAsync(int bookingId, string doctorId, UpdateBookingDTO request)
        {
            var booking = await _context.Bookings.FirstOrDefaultAsync(b => b.BookingId == bookingId && b.DoctorId == doctorId);

            if (booking == null || booking.Status != "Confirmed")
            {
                return false;
            }

            booking.Status = "Completed"; 
            await _context.SaveChangesAsync();
            return true;
        }

  
        public async Task<bool> ConfirmAppointmentAsync(int bookingId)
        {
            var booking = await _context.Bookings.FirstOrDefaultAsync(b => b.BookingId == bookingId && b.Status == "Pending");

            if (booking == null)
            {
                return false;
            }

            var isSlotTaken = await _context.Bookings.AnyAsync(b => b.DoctorId == booking.DoctorId && b.TimeSlot == booking.TimeSlot && b.Status == "Confirmed");

            if (isSlotTaken)
            {
                return false; 
            }

            booking.Status = "Confirmed"; 
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> CancelAppointmentAsync(int bookingId)
        {
            var booking = await _context.Bookings.FirstOrDefaultAsync(b =>
        b.BookingId == bookingId && (b.Status == "Pending" || b.Status == "Confirmed"));

            if (booking == null)
            {
                return false; 
            }
            booking.Status = "Available";
            booking.CustomerId = null;
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> CustomerCancelAppointmentAsync(int bookingId)
        {
            var booking = await _context.Bookings.FirstOrDefaultAsync(b =>
                b.BookingId == bookingId && (b.Status == "Pending" || b.Status == "Confirmed"));

            if (booking == null)
            {
                return false;
            }

            if (booking.Status == "Confirmed")
            {
                throw new InvalidOperationException("Lịch đã được xác nhận, bạn không thể tự hủy. Xin vui lòng liên hệ nhân viên hỗ trợ!");
            }

            booking.Status = "Available";
            booking.CustomerId = null;
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task CreateDoctorBookingsAsync(string doctorId)
        {
            DateTime today = DateTime.Today;
            List<DateTime> timeSlots = new List<DateTime>();

            for (int day = 0; day < 6; day++) 
            {
                DateTime date = today.AddDays(day);
                timeSlots.AddRange(new List<DateTime>
                {
                    date.AddHours(8), date.AddHours(9),
                    date.AddHours(10), date.AddHours(11),
                    date.AddHours(13), date.AddHours(14),
                    date.AddHours(15), date.AddHours(16)
                });
            }

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

        public async Task<List<BookingDTO>> GetPendingAppointmentsAsync()
        {
            return await _context.Bookings
                .Where(b => b.Status == "Pending")
                .OrderBy(b => b.TimeSlot)
                .Join(_context.Users,
                      booking => booking.DoctorId,
                      user => user.Id,
                      (booking, user) => new BookingDTO
                      {
                          BookingId = booking.BookingId,
                          TimeSlot = booking.TimeSlot,
                          Status = booking.Status,
                          CustomerId = booking.CustomerId,
                          DoctorAvatar = user.Avatar,
                          DoctorFirstName = user.FirstName,
                          DoctorLastName = user.LastName
                      })
                .ToListAsync();
        }

        public async Task<List<DoctorDTO>> GetAllDoctorsAsync()
        {
            var doctors = await _context.Users
                .Where(u => _context.UserRoles
                    .Where(ur => _context.Roles
                        .Where(r => r.Name == "Doctor")
                        .Select(r => r.Id)
                        .Contains(ur.RoleId))
                    .Select(ur => ur.UserId)
                    .Contains(u.Id)
                )
                .Select(u => new DoctorDTO
                {
                    Id=u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    Avatar = u.Avatar
                })
                .ToListAsync();

            return doctors;
        }


        public async Task<bool> UpdateBookingDetailsAsync(int bookingId, string doctorId, UpdateBookingDTO request)
        {
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.BookingId == bookingId && b.DoctorId == doctorId);

            if (booking == null || booking.Status != "Confirmed") 
            {
                return false;
            }

            booking.Note = request.Note;
            booking.Prescription = request.Prescription;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ResultBookingDTO> GetResultBookingAsync(int bookingId, string customerId)
        {
            var booking = await _context.Bookings
                .Where(b => b.BookingId == bookingId && b.CustomerId == customerId && b.Status == "Confirmed")
                .Select(b => new ResultBookingDTO
                {
                    Prescription = b.Prescription,
                    Note = b.Note,
                    TimeSlot = b.TimeSlot
                })
                .FirstOrDefaultAsync();

            return booking;
        }

        public async Task<List<BookingDTO>> GetAllConfirmedAppointmentsAsync()
        {
            return await _context.Bookings
                .Where(b => b.Status == "Confirmed")
                .OrderBy(b => b.TimeSlot)
                .Join(_context.Users,
                      booking => booking.DoctorId,
                      user => user.Id,
                      (booking, user) => new BookingDTO
                      {
                          BookingId = booking.BookingId,
                          TimeSlot = booking.TimeSlot,
                          Status = booking.Status,
                          CustomerId = booking.CustomerId,
                          DoctorAvatar = user.Avatar,
                          DoctorFirstName = user.FirstName,
                          DoctorLastName = user.LastName
                      })
                .ToListAsync();
        }




    }
}
