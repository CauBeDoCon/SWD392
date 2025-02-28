using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWD392.DTOs;
using SWD392.Models;
using SWD392.Repositories;
using System.Transactions;

namespace SWD392.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShippingMethodsController : ControllerBase
    {
        private readonly IShippingMethodRepository _shippingMethodRepo;

        public ShippingMethodsController(IShippingMethodRepository repo)
        {
            _shippingMethodRepo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllShippingMethods([FromQuery] int? pageNumber, [FromQuery] int? pageSize)
        {
            int currentPage = pageNumber ?? 1;
            int currentSize = pageSize ?? 10;

            var result = await _shippingMethodRepo.GetAllShippingMethodsAsync(currentPage, currentSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetShippingMethodById(int id)
        {
            var ShippingMethod = await _shippingMethodRepo.GetShippingMethodsAsync(id);
            return ShippingMethod == null ? NotFound() : Ok(ShippingMethod);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddNewShippingMethod([FromBody] UpdateShippingMethodDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var model = new ShippingMethodModel
            {
                Name = dto.Name,
                ShippingCost = dto.ShippingCost
            };

            var newShippingMethodId = await _shippingMethodRepo.AddShippingMethodAsync(model);
            var ShippingMethod = await _shippingMethodRepo.GetShippingMethodsAsync(newShippingMethodId);
            return ShippingMethod == null ? NotFound() : Ok(ShippingMethod);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateShippingMethod(int id, [FromBody] UpdateShippingMethodDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var existingShippingMethod = await _shippingMethodRepo.GetShippingMethodsAsync(id);
            if (existingShippingMethod == null)
            {
                return NotFound($"Không tìm thấy phương thức vận chuyển có ID = {id}");
            }

            existingShippingMethod.Name = dto.Name;
            existingShippingMethod.ShippingCost = dto.ShippingCost;

            await _shippingMethodRepo.UpdateShippingMethodAsync(id, existingShippingMethod);
            return Ok(existingShippingMethod);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteShippingMethod([FromRoute] int id)
        {
            var message = await _shippingMethodRepo.DeleteShippingMethodAsync(id);
            return Ok(new { message });
        }
    }
}
