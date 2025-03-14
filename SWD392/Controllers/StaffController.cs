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
    public class StaffController : ControllerBase
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IDoctorRepository _doctorRepository;

        public StaffController(IBookingRepository bookingRepository, IDoctorRepository doctorRepository)
        {
            _bookingRepository = bookingRepository;
            _doctorRepository = doctorRepository;
        }

        [HttpGet("GetPendingAppointments")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> GetPendingAppointments()
        {
            var pendingBookings = await _bookingRepository.GetPendingAppointmentsAsync();
            return Ok(pendingBookings);
        }
        [HttpGet("GetAllConfirmedAppointments")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> GetAllConfirmedAppointments()
        {
            var confirmedBookings = await _bookingRepository.GetAllConfirmedAppointmentsAsync();
            return Ok(confirmedBookings);
        }


        [HttpPut("ConfirmAppointment/{bookingId}")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> ConfirmAppointment(int bookingId)
        {
            var success = await _bookingRepository.ConfirmAppointmentAsync(bookingId);
            if (!success)
            {
                return BadRequest(new { Message = "Không thể xác nhận lịch hẹn. Có thể lịch này đã trùng giờ." });
            }
            return Ok(new { Message = "Lịch hẹn đã được xác nhận." });
        }

        [HttpPut("CancelAppointment/{bookingId}")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> CancelAppointment(int bookingId)
        {
            var success = await _bookingRepository.CancelAppointmentAsync(bookingId);
            if (!success)
            {
                return BadRequest(new { Message = "Không thể hủy lịch hẹn. Có thể lịch này đã bị hủy hoặc không tồn tại." });
            }
            return Ok(new { Message = "Lịch hẹn đã được hủy." });
        }



        [HttpDelete("DeleteDoctor/{doctorId}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> DeleteDoctor(string doctorId)
        {
            var success = await _doctorRepository.DeleteDoctorAsync(doctorId);
            if (!success)
            {
                return BadRequest(new { Message = "Không thể xóa bác sĩ." });
            }
            return Ok(new { Message = "Bác sĩ đã bị xóa." });
        }
    }
}
