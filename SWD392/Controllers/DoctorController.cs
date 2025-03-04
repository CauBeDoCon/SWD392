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

        public DoctorController(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        // ✅ 1. Xem lịch làm việc
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

        // ✅ 2. Cập nhật kết quả khám (đánh dấu completed)
        [HttpPut("CompleteAppointment/{bookingId}")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> CompleteAppointment(int bookingId, [FromBody] AppointmentCompletionDTO request)
        {
            var doctorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(doctorId))
            {
                return Unauthorized("Không xác định được tài khoản bác sĩ.");
            }

            var success = await _bookingRepository.CompleteAppointmentAsync(bookingId, doctorId, request);
            if (!success)
            {
                return BadRequest(new { Message = "Không thể cập nhật kết quả khám." });
            }

            return Ok(new { Message = "Lịch khám đã hoàn thành." });
        }
    }
}
