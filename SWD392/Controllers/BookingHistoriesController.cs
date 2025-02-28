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
    public class BookingHistorysController : ControllerBase
    {
        private readonly IBookingHistoryRepository _bookingHistoryRepo;

        public BookingHistorysController(IBookingHistoryRepository repo)
        {
            _bookingHistoryRepo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBookingHistorys([FromQuery] int? pageNumber, [FromQuery] int? pageSize)
        {
            int currentPage = pageNumber ?? 1;
            int currentSize = pageSize ?? 10;

            var result = await _bookingHistoryRepo.GetAllBookingHistoriesAsync(currentPage, currentSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingHistoryById(int id)
        {
            var BookingHistory = await _bookingHistoryRepo.GetBookingHistoriesAsync(id);
            return BookingHistory == null ? NotFound() : Ok(BookingHistory);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddNewBookingHistory([FromBody] UpdateBookingHistoryDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var model = new BookingHistoryModel
            {
                BookingId = dto.BookingId,
                CreatedAt = dto.CreatedAt,
                Type = dto.Type,
                Status = dto.Status
            };

            var newBookingHistoryId = await _bookingHistoryRepo.AddBookingHistoryAsync(model);
            var BookingHistory = await _bookingHistoryRepo.GetBookingHistoriesAsync(newBookingHistoryId);
            return BookingHistory == null ? NotFound() : Ok(BookingHistory);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateBookingHistory(int id, [FromBody] UpdateBookingHistoryDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var existingBookingHistory = await _bookingHistoryRepo.GetBookingHistoriesAsync(id);
            if (existingBookingHistory == null)
            {
                return NotFound($"Không tìm thấy lịch sử đặt lịch có ID = {id}");
            }

            existingBookingHistory.BookingId = dto.BookingId;
            existingBookingHistory.CreatedAt = dto.CreatedAt;
            existingBookingHistory.Type = dto.Type;
            existingBookingHistory.Status = dto.Status;

            await _bookingHistoryRepo.UpdateBookingHistoryAsync(id, existingBookingHistory);
            return Ok(existingBookingHistory);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteBookingHistory([FromRoute] int id)
        {
            var message = await _bookingHistoryRepo.DeleteBookingHistoryAsync(id);
            return Ok(new { message });
        }
    }
}
