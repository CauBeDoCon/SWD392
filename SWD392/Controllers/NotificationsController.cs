using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWD392.DTOs;
using SWD392.Models;
using SWD392.Repositories;

namespace SWD392.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationRepository _notificationRepo;

        public NotificationsController(INotificationRepository repo)
        {
            _notificationRepo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNotifications([FromQuery] int? pageNumber, [FromQuery] int? pageSize)
        {
            int currentPage = pageNumber ?? 1;
            int currentSize = pageSize ?? 10;

            var result = await _notificationRepo.GetAllNotificationsAsync(currentPage, currentSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNotificationById(int id)
        {
            var Notification = await _notificationRepo.GetNotificationsAsync(id);
            return Notification == null ? NotFound() : Ok(Notification);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddNewNotification([FromBody] UpdateNotificationDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var model = new NotificationModel
            {
                Message = dto.Message,
                CreatedDate = dto.CreatedDate,
                Status = dto.Status,
                UserId = dto.UserId
            };

            var newNotificationId = await _notificationRepo.AddNotificationAsync(model);
            var Notification = await _notificationRepo.GetNotificationsAsync(newNotificationId);
            return Notification == null ? NotFound() : Ok(Notification);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateNotification(int id, [FromBody] UpdateNotificationDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var existingNotification = await _notificationRepo.GetNotificationsAsync(id);
            if (existingNotification == null)
            {
                return NotFound($"Không tìm thấy thông báo có ID = {id}");
            }

            existingNotification.Message = dto.Message;
            existingNotification.CreatedDate = dto.CreatedDate;
            existingNotification.Status = dto.Status;
            existingNotification.UserId = dto.UserId;

            await _notificationRepo.UpdateNotificationAsync(id, existingNotification);
            return Ok(existingNotification);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteNotification([FromRoute] int id)
        {
            var message = await _notificationRepo.DeleteNotificationAsync(id);
            return Ok(new { message });
        }
    }
}
