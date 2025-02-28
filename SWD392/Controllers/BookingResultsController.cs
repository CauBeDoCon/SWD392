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
    public class BookingResultsController : ControllerBase
    {
        private readonly IBookingResultRepository _bookingResultRepo;

        public BookingResultsController(IBookingResultRepository repo)
        {
            _bookingResultRepo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBookingResults([FromQuery] int? pageNumber, [FromQuery] int? pageSize)
        {
            int currentPage = pageNumber ?? 1;
            int currentSize = pageSize ?? 10;

            var result = await _bookingResultRepo.GetAllBookingResultsAsync(currentPage, currentSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingResultById(int id)
        {
            var BookingResult = await _bookingResultRepo.GetBookingResultsAsync(id);
            return BookingResult == null ? NotFound() : Ok(BookingResult);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddNewBookingResult([FromBody] UpdateBookingResultDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var model = new BookingResultModel
            {
                BookingId = dto.BookingId,
                CreatedAt = dto.CreatedAt,
                ResultsOfDoctor = dto.ResultsOfDoctor,
                Status = dto.Status,
                StatusSkin = dto.StatusSkin,
                StatusAcne = dto.StatusAcne
            };

            var newBookingResultId = await _bookingResultRepo.AddBookingResultAsync(model);
            var BookingResult = await _bookingResultRepo.GetBookingResultsAsync(newBookingResultId);
            return BookingResult == null ? NotFound() : Ok(BookingResult);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateBookingResult(int id, [FromBody] UpdateBookingResultDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var existingBookingResult = await _bookingResultRepo.GetBookingResultsAsync(id);
            if (existingBookingResult == null)
            {
                return NotFound($"Không tìm thấy kết quả đặt lịch có ID = {id}");
            }

            existingBookingResult.BookingId = dto.BookingId;
            existingBookingResult.CreatedAt = dto.CreatedAt;
            existingBookingResult.ResultsOfDoctor = dto.ResultsOfDoctor;
            existingBookingResult.Status = dto.Status;
            existingBookingResult.StatusSkin = dto.StatusSkin;
            existingBookingResult.StatusAcne = dto.StatusAcne;

            await _bookingResultRepo.UpdateBookingResultAsync(id, existingBookingResult);
            return Ok(existingBookingResult);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteBookingResult([FromRoute] int id)
        {
            var message = await _bookingResultRepo.DeleteBookingResultAsync(id);
            return Ok(new { message });
        }
    }
}
