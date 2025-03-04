using SWD392.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWD392.Repositories
{
    public interface IBookingRepository
    {
        Task<List<BookingDTO>> GetAvailableBookingsAsync(string doctorId);
        Task<bool> BookAppointmentAsync(BookingRequestDTO request, string customerUsername);
        Task<List<BookingDTO>> GetDoctorBookingsAsync(string doctorId);

        Task CreateDoctorBookingsAsync(string doctorId); 


    }
}
