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
    public class ShippingsController : ControllerBase
    {
        private readonly IShippingRepository _shippingRepo;

        public ShippingsController(IShippingRepository repo)
        {
            _shippingRepo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllShippings([FromQuery] int? pageNumber, [FromQuery] int? pageSize)
        {
            int currentPage = pageNumber ?? 1;
            int currentSize = pageSize ?? 10;

            var result = await _shippingRepo.GetAllShippingsAsync(currentPage, currentSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetShippingById(int id)
        {
            var Shipping = await _shippingRepo.GetShippingsAsync(id);
            return Shipping == null ? NotFound() : Ok(Shipping);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddNewShipping([FromBody] UpdateShippingDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var model = new ShippingModel
            {
                ShippingAddress = dto.ShippingAddress,
                EstimatedDeliveryDate = dto.EstimatedDeliveryDate,
                ShippingCost = dto.ShippingCost,
                ShippingMethodId = dto.ShippingMethodId,
                OrderId = dto.OrderId
            };

            var newShippingId = await _shippingRepo.AddShippingAsync(model);
            var Shipping = await _shippingRepo.GetShippingsAsync(newShippingId);
            return Shipping == null ? NotFound() : Ok(Shipping);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateShipping(int id, [FromBody] UpdateShippingDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var existingShipping = await _shippingRepo.GetShippingsAsync(id);
            if (existingShipping == null)
            {
                return NotFound($"Không tìm thấy vận chuyển có ID = {id}");
            }

            existingShipping.ShippingAddress = dto.ShippingAddress;
            existingShipping.EstimatedDeliveryDate = dto.EstimatedDeliveryDate;
            existingShipping.ShippingCost = dto.ShippingCost;
            existingShipping.ShippingMethodId = dto.ShippingMethodId;
            existingShipping.OrderId = dto.OrderId;

            await _shippingRepo.UpdateShippingAsync(id, existingShipping);
            return Ok(existingShipping);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteShipping([FromRoute] int id)
        {
            var message = await _shippingRepo.DeleteShippingAsync(id);
            return Ok(new { message });
        }
    }
}
