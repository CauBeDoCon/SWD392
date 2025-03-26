using Microsoft.EntityFrameworkCore;
using SWD392.DB;
using SWD392.DTOs;
using System.Collections.Generic;
using System.Globalization;
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



        public async Task<List<DoctorScheduleDTO>> GetDoctorScheduleAsync(string doctorId)
        {
            var result = await (
                from b in _context.Bookings
                join u in _context.Users on b.CustomerId equals u.Id
                where b.DoctorId == doctorId
                      && b.Status == "Confirmed"
                      && b.CustomerId != null
                select new DoctorScheduleDTO
                {
                    BookingId = b.BookingId,
                    TimeSlot = b.TimeSlot,
                    Status = b.Status,
                    CustomerId = u.Id,
                    CustomerName = u.FirstName + " " + u.LastName,
                    CustomerPhone = u.PhoneNumber,
                    CustomerAvatar = u.Avatar
                }).ToListAsync();

            return result;
        }

        public async Task<List<DoctorScheduleDTO>> SearchBookingByPhoneAsync(string phoneNumber)
        {
            var result = await (
                from b in _context.Bookings
                join u in _context.Users on b.CustomerId equals u.Id
                where b.Status == "Confirmed"
                      && u.PhoneNumber == phoneNumber
                select new DoctorScheduleDTO
                {
                    BookingId = b.BookingId,
                    TimeSlot = b.TimeSlot,
                    Status = b.Status,
                    CustomerId = u.Id,
                    CustomerName = u.FirstName + " " + u.LastName,
                    CustomerPhone = u.PhoneNumber,
                    CustomerAvatar = u.Avatar
                }).ToListAsync();

            return result;
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


        public async Task CreateDoctorBookingsAsync(string doctorId, int numberOfDays = 7)
        {
            DateTime today = DateTime.Today;
            List<DateTime> timeSlots = new List<DateTime>();

            for (int day = 0; day < numberOfDays; day++)
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
                bool exists = await _context.Bookings
                    .AnyAsync(b => b.DoctorId == doctorId && b.TimeSlot == slot);

                if (!exists)
                {
                    bookings.Add(new Booking
                    {
                        DoctorId = doctorId,
                        TimeSlot = slot,
                        Status = "Available"
                    });
                }
            }

            if (bookings.Count > 0)
            {
                _context.Bookings.AddRange(bookings);
                await _context.SaveChangesAsync();
            }
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


        public async Task<bool> HasScheduleForDateAsync(string doctorId, DateTime date)
        {
            return await _context.Bookings.AnyAsync(b =>
                b.DoctorId == doctorId &&
                b.TimeSlot.Date == date.Date);
        }

        public async Task<bool> DeleteDoctorBookingsForDateAsync(string doctorId, DateTime date)
        {
            var bookingsToDelete = await _context.Bookings
                .Where(b => b.DoctorId == doctorId && b.TimeSlot.Date == date)
                .ToListAsync();

            if (bookingsToDelete.Any())
            {
                _context.Bookings.RemoveRange(bookingsToDelete);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<int> GetPendingBookingCountAsync()
        {
            return await _context.Bookings.CountAsync(b => b.Status == "Pending");
        }

        public async Task<int> GetConfirmedBookingCountAsync()
        {
            return await _context.Bookings.CountAsync(b => b.Status == "Confirmed");
        }

        public async Task<BookingFrequencyDTO> GetConfirmedBookingFrequencyByDayAsync(DateTime startDate, DateTime endDate)
        {
            var data = await _context.Bookings
                .Where(b => b.TimeSlot.Date >= startDate.Date && b.TimeSlot.Date <= endDate.Date && b.Status == "Confirmed")
                .GroupBy(b => b.TimeSlot.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Count = g.Count()
                })
                .OrderBy(g => g.Date)
                .ToListAsync();

            return new BookingFrequencyDTO
            {
                Labels = data.Select(d => d.Date.ToString("yyyy-MM-dd")).ToList(),
                Data = data.Select(d => d.Count).ToList()
            };
        }


        public async Task<BookingFrequencyDTO> GetConfirmedBookingFrequencyByWeekAsync(DateTime startDate, DateTime endDate)
        {
            var result = _context.Bookings
                .Where(b => b.TimeSlot.Date >= startDate.Date && b.TimeSlot.Date <= endDate.Date && b.Status == "Confirmed")
                .AsEnumerable() 
                .GroupBy(b => new
                {
                    Year = b.TimeSlot.Year,
                    Week = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(
                        b.TimeSlot, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday)
                })
                .Select(g => new
                {
                    Week = g.Key.Week,
                    Year = g.Key.Year,
                    Count = g.Count()
                })
                .OrderBy(g => g.Year).ThenBy(g => g.Week)
                .ToList(); 

            return new BookingFrequencyDTO
            {
                Labels = result.Select(d => $"Week {d.Week} - {d.Year}").ToList(),
                Data = result.Select(d => d.Count).ToList()
            };
        }


        public async Task<BookingFrequencyDTO> GetConfirmedBookingFrequencyByMonthAsync(int year)
        {
            var data = await _context.Bookings
                .Where(b => b.TimeSlot.Year == year && b.Status == "Confirmed")
                .GroupBy(b => b.TimeSlot.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    Count = g.Count()
                })
                .OrderBy(g => g.Month)
                .ToListAsync();

            return new BookingFrequencyDTO
            {
                Labels = data.Select(d => $"Tháng {d.Month}").ToList(),
                Data = data.Select(d => d.Count).ToList()
            };
        }


    }
}
