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
    public class ReturnsController : ControllerBase
    {
        private readonly IReturnRepository _returnRepo;

        public ReturnsController(IReturnRepository repo)
        {
            _returnRepo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllReturns([FromQuery] int? pageNumber, [FromQuery] int? pageSize)
        {
            int currentPage = pageNumber ?? 1;
            int currentSize = pageSize ?? 10;

            var result = await _returnRepo.GetAllReturnsAsync(currentPage, currentSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReturnById(int id)
        {
            var Return = await _returnRepo.GetReturnsAsync(id);
            return Return == null ? NotFound() : Ok(Return);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddNewReturn([FromBody] UpdateReturnDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var model = new ReturnModel
            {
                Reason = dto.Reason,
                ReturnDate = dto.ReturnDate,
                Status = dto.Status,
                ShippingId = dto.ShippingId,
                OrderId = dto.OrderId,
                ProductId = dto.ProductId
            };

            var newReturnId = await _returnRepo.AddReturnAsync(model);
            var Return = await _returnRepo.GetReturnsAsync(newReturnId);
            return Return == null ? NotFound() : Ok(Return);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateReturn(int id, [FromBody] UpdateReturnDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var existingReturn = await _returnRepo.GetReturnsAsync(id);
            if (existingReturn == null)
            {
                return NotFound($"Không tìm thấy hoàn trả có ID = {id}");
            }

            existingReturn.Reason = dto.Reason;
            existingReturn.ReturnDate = dto.ReturnDate;
            existingReturn.Status = dto.Status;
            existingReturn.ShippingId = dto.ShippingId;
            existingReturn.OrderId = dto.OrderId;
            existingReturn.ProductId = dto.ProductId;

            await _returnRepo.UpdateReturnAsync(id, existingReturn);
            return Ok(existingReturn);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteReturn([FromRoute] int id)
        {
            var message = await _returnRepo.DeleteReturnAsync(id);
            return Ok(new { message });
        }
    }
}
