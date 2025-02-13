using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWD392.DTOs;
using SWD392.Models;
using SWD392.Repositories;

namespace SWD392.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitProductsController : ControllerBase
    {
        private readonly IUnitProductRepository _unitProductRepo;

        public UnitProductsController(IUnitProductRepository repo)
        {
            _unitProductRepo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUnitProducts()
        {
            return Ok(await _unitProductRepo.GetAllUnitProductsAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUnitProductById(int id)
        {
            var unitProduct = await _unitProductRepo.GetUnitProductsAsync(id);
            return unitProduct == null ? NotFound() : Ok(unitProduct);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddNewUnitProduct([FromBody] UpdateUnitProductDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var model = new UnitProductModel
            {
                Price = dto.Price,
                ProductId = dto.ProductId,
                UnitId = dto.UnitId
            };

            var newUnitProductId = await _unitProductRepo.AddUnitProductAsync(model);
            var unitProduct = await _unitProductRepo.GetUnitProductsAsync(newUnitProductId);
            return unitProduct == null ? NotFound() : Ok(unitProduct);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateUnitProduct(int id, [FromBody] UpdateUnitProductDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var existingUnitProduct = await _unitProductRepo.GetUnitProductsAsync(id);
            if (existingUnitProduct == null)
            {
                return NotFound($"Không tìm thấy đơn vị sản phẩm có ID = {id}");
            }

            existingUnitProduct.Price = dto.Price;
            existingUnitProduct.ProductId = dto.ProductId;
            existingUnitProduct.UnitId = dto.UnitId;

            await _unitProductRepo.UpdateUnitProductAsync(id, existingUnitProduct);
            return Ok(existingUnitProduct);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteUnitProduct([FromRoute] int id)
        {
            await _unitProductRepo.DeleteUnitProductAsync(id);
            return Ok();
        }
    }
}
