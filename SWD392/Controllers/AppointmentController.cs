using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using SWD392.DTOs;

namespace SWD392.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public AppointmentController(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        [HttpGet("GetAllAppointments")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> GetAllAppointments()
        {
            var appointments = await _appointmentRepository.GetAllAppointmentsAsync();
            return Ok(appointments);
        }

        [HttpGet("GetAppointment/{appointmentId}")]
        [Authorize]
        public async Task<IActionResult> GetAppointment(int appointmentId)
        {
            var appointment = await _appointmentRepository.GetAppointmentByIdAsync(appointmentId);
            if (appointment == null)
            {
                return NotFound(new { Message = "Không tìm thấy lịch hẹn." });
            }
            return Ok(appointment);
        }

        [HttpPost("CreateAppointment")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CreateAppointment([FromBody] AppointmentDTO appointmentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { Message = "Không thể xác định danh tính khách hàng." });
            }

            var newAppointment = await _appointmentRepository.CreateAppointmentAsync(userId, appointmentDto);
            if (newAppointment == null)
            {
                return BadRequest(new { Message = "Không thể tạo lịch hẹn. Kiểm tra lại PackageId." });
            }

            return CreatedAtAction(nameof(GetAppointment), new { appointmentId = newAppointment.Id }, newAppointment);
        }

        [HttpPut("ConfirmAppointment/{appointmentId}")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> ConfirmAppointment(int appointmentId)
        {
            var result = await _appointmentRepository.ConfirmAppointmentAsync(appointmentId);
            if (!result.Success)
            {
                return BadRequest(new { Message = result.Message });
            }
            return Ok(new { Message = "Lịch hẹn đã được xác nhận." });
        }

        [HttpPut("CancelAppointment/{appointmentId}")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> CancelAppointment(int appointmentId)
        {
            var result = await _appointmentRepository.CancelAppointmentAsync(appointmentId);
            if (!result.Success)
            {
                return BadRequest(new { Message = result.Message });
            }
            return Ok(new { Message = "Lịch hẹn đã bị hủy." });
        }
    }
}
