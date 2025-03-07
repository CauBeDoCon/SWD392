using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWD392.DB;
using SWD392.Helpers;
using System.Security.Claims;

namespace SWD392.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Manager")] 
    public class ManagerController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ManagerController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }


        [HttpGet("GetAllCustomers")]
        public async Task<IActionResult> GetAllCustomers()
        {
            var customers = await _userManager.GetUsersInRoleAsync(AppRole.Customer);
            return Ok(customers);
        }

        [HttpGet("GetAllStaffs")]
        public async Task<IActionResult> GetAllStaffs()
        {
            var staffs = await _userManager.GetUsersInRoleAsync(AppRole.Staff);
            return Ok(staffs);
        }

        [HttpPut("ToggleUserStatus/{userId}")]
        public async Task<IActionResult> ToggleUserStatus(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new { Message = "Không tìm thấy người dùng." });
            }

            if (!(await _userManager.IsInRoleAsync(user, AppRole.Staff) || await _userManager.IsInRoleAsync(user, AppRole.Customer)))
            {
                return BadRequest(new { Message = "Bạn chỉ có thể thay đổi trạng thái của Staff hoặc Customer." });
            }

            user.Status = user.Status == "Active" ? "Banned" : "Active";
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return StatusCode(500, new { Message = "Cập nhật trạng thái thất bại." });
            }

            return Ok(new
            {
                Message = $"Người dùng {user.Email} đã được chuyển sang trạng thái {user.Status}.",
                NewStatus = user.Status
            });
        }
    }
}
