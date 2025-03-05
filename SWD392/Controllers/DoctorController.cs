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

    }
}
