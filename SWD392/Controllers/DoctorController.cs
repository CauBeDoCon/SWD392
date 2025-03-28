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
    public class DoctorController : ControllerBase
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IAppointmentRepository _appointmentRepository;

        public DoctorController(IBookingRepository bookingRepository, IAppointmentRepository appointmentRepository)
        {
            _bookingRepository = bookingRepository;
            _appointmentRepository = appointmentRepository;
        }

     
        [HttpGet("GetDoctorSchedule")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetDoctorSchedule()
        {
            var doctorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(doctorId))
            {
                return Unauthorized("Không xác định được tài khoản bác sĩ.");
            }

            var schedule = await _bookingRepository.GetDoctorScheduleAsync(doctorId);
            return Ok(schedule);
        }


        [HttpPut("UpdateBookingDetails/{bookingId}")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> UpdateBookingDetails(int bookingId, [FromBody] UpdateBookingDTO request)
        {
            var doctorId = User.Identity.Name; 

            bool success = await _bookingRepository.UpdateBookingDetailsAsync(bookingId, doctorId, request);

            if (!success)
            {
                return BadRequest(new { Message = "Không thể cập nhật thông tin. Kiểm tra quyền truy cập hoặc trạng thái booking!" });
            }

            return Ok(new { Message = "Cập nhật thông tin thành công!" });
        }


        [HttpGet("GetDoctorAppointments")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetDoctorAppointments()
        {
            var doctorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(doctorId))
            {
                return Unauthorized(new { Message = "Không xác định được tài khoản bác sĩ." });
            }

            var appointments = await _appointmentRepository.GetDoctorAppointmentsAsync(doctorId);
            return Ok(appointments);
        }

        [HttpGet("SearchBookingByPhone")]
        [Authorize(Roles = "Doctor,Staff")]
        public async Task<IActionResult> SearchBookingByPhone([FromQuery] string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
                return BadRequest(new { Message = "Vui lòng nhập số điện thoại." });

            var result = await _bookingRepository.SearchBookingByPhoneAsync(phoneNumber);

            if (result == null || !result.Any())
                return NotFound(new { Message = "Không tìm thấy lịch hẹn nào với số điện thoại này." });

            return Ok(result);
        }

    }
}
