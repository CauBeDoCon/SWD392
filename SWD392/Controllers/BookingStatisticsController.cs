using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWD392.Repositories;
using System.Threading.Tasks;

namespace SWD392.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingStatisticsController : ControllerBase
    {
        private readonly IBookingRepository _bookingRepository;

        public BookingStatisticsController(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        [HttpGet("PendingBookingCount")]
        public async Task<IActionResult> GetPendingBookingCount()
        {
            int count = await _bookingRepository.GetPendingBookingCountAsync();
            return Ok(new { PendingCount = count });
        }

        [HttpGet("ConfirmedBookingCount")]
        public async Task<IActionResult> GetConfirmedBookingCount()
        {
            int count = await _bookingRepository.GetConfirmedBookingCountAsync();
            return Ok(new { ConfirmedCount = count });
        }

        [HttpGet("ConfirmedBookingFrequencyByDay")]
        public async Task<IActionResult> GetConfirmedBookingFrequencyByDay([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var result = await _bookingRepository.GetConfirmedBookingFrequencyByDayAsync(startDate, endDate);
            return Ok(result);
        }

        [HttpGet("ConfirmedBookingFrequencyByWeek")]
        public async Task<IActionResult> GetConfirmedBookingFrequencyByWeek([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var result = await _bookingRepository.GetConfirmedBookingFrequencyByWeekAsync(startDate, endDate);
            return Ok(result);
        }

        [HttpGet("ConfirmedBookingFrequencyByMonth")]
        public async Task<IActionResult> GetConfirmedBookingFrequencyByMonth([FromQuery] int year)
        {
            var result = await _bookingRepository.GetConfirmedBookingFrequencyByMonthAsync(year);
            return Ok(result);
        }



    }
}
