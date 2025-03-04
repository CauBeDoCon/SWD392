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

        // ✅ 1. Xem lịch trống của bác sĩ
        [HttpGet("GetAvailableBookings/{doctorId}")]
        public async Task<IActionResult> GetAvailableBookings(string doctorId)
        {
            var slots = await _bookingRepository.GetAvailableBookingsAsync(doctorId);
            return Ok(slots);
        }

        // ✅ 2. Đặt lịch hẹn (trạng thái "Pending")
        [HttpPost("RequestAppointment")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> RequestAppointment([FromBody] BookingRequestDTO request)
        {
            var customerUsername = User.Identity.Name;
            if (string.IsNullOrEmpty(customerUsername))
            {
                return Unauthorized(new { Message = "Không xác định được tài khoản khách hàng!" });
            }

            bool success = await _bookingRepository.RequestAppointmentAsync(request, customerUsername);

            if (!success)
            {
                return BadRequest(new { Message = "Lịch này đã được đặt hoặc không tồn tại!" });
            }

            return Ok(new { Message = "Đặt lịch thành công! Chờ xác nhận từ Staff.", BookingId = request.BookingId });
        }

        // ✅ 3. Xem lịch hẹn của khách hàng
        [HttpGet("GetCustomerAppointments")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetCustomerAppointments()
        {
            var customerUsername = User.Identity.Name;
            if (string.IsNullOrEmpty(customerUsername))
            {
                return Unauthorized("Không xác định được tài khoản khách hàng.");
            }

            var appointments = await _bookingRepository.GetCustomerAppointmentsAsync(customerUsername);
            return Ok(appointments);
        }

        // ✅ 4. Hủy lịch hẹn (chỉ trước 24h)
        [HttpPut("CancelAppointment/{bookingId}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CancelAppointment(int bookingId)
        {
            var success = await _bookingRepository.CancelAppointmentAsync(bookingId, User.Identity.Name);
            if (!success)
            {
                return BadRequest(new { Message = "Không thể hủy lịch hẹn, vui lòng liên hệ Staff." });
            }

            return Ok(new { Message = "Hủy lịch thành công." });
        }
    }
}
