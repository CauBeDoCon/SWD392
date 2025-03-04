using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWD392.DTOs;
using SWD392.Repositories;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SWD392.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingRepository _bookingRepository;

        public BookingController(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

  
        [HttpGet("GetAvailableBookings/{doctorId}")]
        public async Task<IActionResult> GetAvailableBookings(string doctorId)
        {
            var slots = await _bookingRepository.GetAvailableBookingsAsync(doctorId);
            return Ok(slots);
        }

      
        [HttpPost("BookAppointment")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> BookAppointment([FromBody] BookingRequestDTO request)
        {
            var customerUsername = User.Identity.Name;
            if (string.IsNullOrEmpty(customerUsername))
            {
                return Unauthorized(new { Message = "Không xác định được tài khoản khách hàng!" });
            }

            bool success = await _bookingRepository.BookAppointmentAsync(request, customerUsername);

            if (!success)
            {
                return BadRequest(new { Message = "Lịch này đã được đặt hoặc không tồn tại!" });
            }

            return Ok(new { Message = "Đặt lịch thành công!", BookingId = request.BookingId });
        }

       
        [HttpGet("GetDoctorBookings")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetDoctorBookings()
        {
            var doctorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(doctorId))
            {
                return Unauthorized("Không xác định được ID của bác sĩ.");
            }

            var bookings = await _bookingRepository.GetDoctorBookingsAsync(doctorId);
            return Ok(bookings);
        }
    }
}
