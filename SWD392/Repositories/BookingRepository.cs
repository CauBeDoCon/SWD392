﻿using Microsoft.EntityFrameworkCore;
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

       
        public async Task<bool> RequestAppointmentAsync(BookingRequestDTO request, string customerUsername)
        {
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.BookingId == request.BookingId && b.Status == "Available");

            if (booking == null)
            {
                return false;
            }

            booking.CustomerUsername = customerUsername;
            booking.Status = "Pending"; 

            await _context.SaveChangesAsync();
            return true;
        }

    
        public async Task<List<BookingDTO>> GetCustomerAppointmentsAsync(string customerUsername)
        {
            return await _context.Bookings
                .Where(b => b.CustomerUsername == customerUsername)
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


        public async Task<bool> CancelAppointmentAsync(int bookingId, string customerUsername)
        {
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.BookingId == bookingId && b.CustomerUsername == customerUsername);

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

     
        public async Task<bool> CompleteAppointmentAsync(int bookingId, string doctorId, AppointmentCompletionDTO request)
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

            booking.Status = "Cancelled";
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
                .Select(b => new BookingDTO
                {
                    BookingId = b.BookingId,
                    TimeSlot = b.TimeSlot,
                    Status = b.Status,
                    CustomerUsername = b.CustomerUsername
                })
                .ToListAsync();
        }

    }
}
