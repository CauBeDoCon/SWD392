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
    public class TimeFramesController : ControllerBase
    {
        private readonly ITimeFrameRepository _timeFrameRepo;

        public TimeFramesController(ITimeFrameRepository repo)
        {
            _timeFrameRepo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTimeFrames([FromQuery] int? pageNumber, [FromQuery] int? pageSize)
        {
            int currentPage = pageNumber ?? 1;
            int currentSize = pageSize ?? 10;

            var result = await _timeFrameRepo.GetAllTimeFramesAsync(currentPage, currentSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTimeFrameById(int id)
        {
            var TimeFrame = await _timeFrameRepo.GetTimeFramesAsync(id);
            return TimeFrame == null ? NotFound() : Ok(TimeFrame);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddNewTimeFrame([FromBody] UpdateTimeFrameDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var model = new TimeFrameModel
            {
                UserId = dto.UserId,
                TimeFrameFrom = dto.TimeFrameFrom,
                TimeFrameTo = dto.TimeFrameTo,
                Status = dto.Status
            };

            var newTimeFrameId = await _timeFrameRepo.AddTimeFrameAsync(model);
            var TimeFrame = await _timeFrameRepo.GetTimeFramesAsync(newTimeFrameId);
            return TimeFrame == null ? NotFound() : Ok(TimeFrame);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateTimeFrame(int id, [FromBody] UpdateTimeFrameDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var existingTimeFrame = await _timeFrameRepo.GetTimeFramesAsync(id);
            if (existingTimeFrame == null)
            {
                return NotFound($"Không tìm thấy khung giờ có ID = {id}");
            }

            existingTimeFrame.UserId = dto.UserId;
            existingTimeFrame.TimeFrameFrom = dto.TimeFrameFrom;
            existingTimeFrame.TimeFrameTo = dto.TimeFrameTo;
            existingTimeFrame.Status = dto.Status;

            await _timeFrameRepo.UpdateTimeFrameAsync(id, existingTimeFrame);
            return Ok(existingTimeFrame);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteTimeFrame([FromRoute] int id)
        {
            var message = await _timeFrameRepo.DeleteTimeFrameAsync(id);
            return Ok(new { message });
        }
    }
}
