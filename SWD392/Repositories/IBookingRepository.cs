using SWD392.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWD392.Repositories
{
    public interface IBookingRepository
    {
        Task<List<BookingDTO>> GetAvailableBookingsAsync(string doctorId);
        Task<bool> RequestAppointmentAsync(BookingRequestDTO request, string customerUsername);
        Task<List<BookingDTO>> GetCustomerAppointmentsAsync(string customerUsername);
        Task<bool> CancelAppointmentAsync(int bookingId, string customerUsername);
        Task<List<BookingDTO>> GetDoctorScheduleAsync(string doctorId);
        Task<bool> CompleteAppointmentAsync(int bookingId, string doctorId, UpdateBookingDTO request);
        Task<bool> ConfirmAppointmentAsync(int bookingId);
        Task<bool> CancelAppointmentAsync(int bookingId);

        Task CreateDoctorBookingsAsync(string doctorId);
        Task<List<DoctorDTO>> GetAllDoctorsAsync();

        Task<List<BookingDTO>> GetPendingAppointmentsAsync();

        Task<bool> UpdateBookingDetailsAsync(int bookingId, string doctorId, UpdateBookingDTO request);


    }
}
 