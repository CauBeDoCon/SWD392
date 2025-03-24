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

            var result = await _appointmentRepository.CreateAppointmentAsync(userId, appointmentDto);
            if (!result.Success)
            {
                return BadRequest(new { Message = result.Message });
            }

            return CreatedAtAction(nameof(GetAppointment), new { appointmentId = result.Appointment.Id }, result.Appointment);
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

        [HttpGet("GetMyAppointment")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetMyAppointment()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { Message = "Không thể xác định danh tính khách hàng." });
            }

            var appointment = await _appointmentRepository.GetCustomerAppointmentAsync(userId);
            if (appointment == null)
            {
                return NotFound(new { Message = "Bạn chưa đăng ký gói nào." });
            }

            return Ok(appointment);
        }

        [HttpPut("Update/{packageTrackingId}")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> UpdatePackageTracking(int packageTrackingId, [FromBody] UpdatePackageTrackingDTO updateDto)
        {
            var success = await _appointmentRepository.UpdatePackageTrackingAsync(packageTrackingId, updateDto);
            if (!success)
            {
                return BadRequest(new { Message = "Không tìm thấy PackageTracking để cập nhật." });
            }
            return Ok(new { Message = "Ghi chú bác sĩ đã được cập nhật." });
        }


        [HttpGet("GetMyTracking")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetMyPackageTracking()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { Message = "Không thể xác định danh tính khách hàng." });
            }

            var trackings = await _appointmentRepository.GetMyPackageTrackingsAsync(userId);

            if (trackings == null || !trackings.Any())
            {
                return NotFound(new { Message = "Không tìm thấy lịch sử điều trị của bạn." });
            }

            return Ok(trackings);
        }
        [HttpGet("GetConfirmedAppointmentsWithTracking")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> GetConfirmedAppointmentsWithTracking()
        {
            var result = await _appointmentRepository.GetConfirmedAppointmentsWithTrackingAsync();
            return Ok(result);
        }


        [HttpGet("GetCustomerScheduleByPhone")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> GetCustomerScheduleByPhone([FromQuery] string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return BadRequest(new { Message = "Vui lòng nhập số điện thoại." });

            var schedule = await _appointmentRepository.GetCustomerScheduleByPhoneAsync(phoneNumber);
            if (schedule == null)
                return NotFound(new { Message = "Không tìm thấy lộ trình điều trị cho khách hàng này." });

            return Ok(schedule);
        }
        [HttpPut("CheckinTreatment/{trackingId}")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> CheckinTreatmentSession(int trackingId, [FromBody] CheckinTrackingDTO dto)
        {
            var success = await _appointmentRepository.CheckinTreatmentSessionAsync(trackingId, dto);
            if (!success)
                return NotFound(new { Message = "Không tìm thấy buổi điều trị để cập nhật." });

            return Ok(new { Message = "Đã check-in bệnh nhân thành công. Trạng thái cập nhật thành 'Done'." });
        }
        [HttpDelete("DeleteCompletedAppointment/{appointmentId}")]
        [Authorize(Roles = "Customer,Staff")]
        public async Task<IActionResult> DeleteCompletedAppointment(int appointmentId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { Message = "Không xác định được người dùng." });

            var result = await _appointmentRepository.DeleteCompletedAppointmentAsync(appointmentId, userId, role);

            if (!result.Success)
                return BadRequest(new { result.Message });

            return Ok(new { result.Message });
        }




    }
}
