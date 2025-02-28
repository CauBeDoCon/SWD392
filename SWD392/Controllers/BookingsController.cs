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
    public class BookingsController : ControllerBase
    {
        private readonly IBookingRepository _bookingRepo;

        public BookingsController(IBookingRepository repo)
        {
            _bookingRepo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBookings([FromQuery] int? pageNumber, [FromQuery] int? pageSize)
        {
            int currentPage = pageNumber ?? 1;
            int currentSize = pageSize ?? 10;

            var result = await _bookingRepo.GetAllBookingsAsync(currentPage, currentSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingById(int id)
        {
            var Booking = await _bookingRepo.GetBookingsAsync(id);
            return Booking == null ? NotFound() : Ok(Booking);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddNewBooking([FromBody] UpdateBookingDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var model = new BookingModel
            {
                Account = dto.Account,
                CreatedAt = dto.CreatedAt,
                UserId = dto.UserId,
                MeetingLink = dto.MeetingLink,
                Status = dto.Status,
                Type = dto.Type,
                TimeFrameId = dto.TimeFrameId,
            };

            var newBookingId = await _bookingRepo.AddBookingAsync(model);
            var Booking = await _bookingRepo.GetBookingsAsync(newBookingId);
            return Booking == null ? NotFound() : Ok(Booking);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateBooking(int id, [FromBody] UpdateBookingDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var existingBooking = await _bookingRepo.GetBookingsAsync(id);
            if (existingBooking == null)
            {
                return NotFound($"Không tìm thấy đặt lịch có ID = {id}");
            }

            existingBooking.Account = dto.Account;
            existingBooking.CreatedAt = dto.CreatedAt;
            existingBooking.UserId = dto.UserId;
            existingBooking.MeetingLink = dto.MeetingLink;
            existingBooking.Status = dto.Status;
            existingBooking.Type = dto.Type;
            existingBooking.TimeFrameId = dto.TimeFrameId;

            await _bookingRepo.UpdateBookingAsync(id, existingBooking);
            return Ok(existingBooking);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteBooking([FromRoute] int id)
        {
            var message = await _bookingRepo.DeleteBookingAsync(id);
            return Ok(new { message });
        }
    }
}
