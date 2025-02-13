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
    public class BrandsController : ControllerBase
    {
        private readonly IBrandRepository _brandRepo;

        public BrandsController(IBrandRepository repo)
        {
            _brandRepo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBrands([FromQuery] int? pageNumber, [FromQuery] int? pageSize)
        {
            int currentPage = pageNumber ?? 1;
            int currentSize = pageSize ?? 10;

            var result = await _brandRepo.GetAllBrandsAsync(currentPage, currentSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBrandById(int id)
        {
            var brand = await _brandRepo.GetBrandsAsync(id);
            return brand == null ? NotFound() : Ok(brand);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddNewBrand([FromBody] UpdateBrandDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var model = new BrandModel
            {
                Name = dto.Name
            };

            var newBrandId = await _brandRepo.AddBrandAsync(model);
            var brand = await _brandRepo.GetBrandsAsync(newBrandId);
            return brand == null ? NotFound() : Ok(brand);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateBrand(int id, [FromBody] UpdateBrandDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var existingBrand = await _brandRepo.GetBrandsAsync(id);
            if (existingBrand == null)
            {
                return NotFound($"Không tìm thấy thương hiệu có ID = {id}");
            }

            existingBrand.Name = dto.Name;

            await _brandRepo.UpdateBrandAsync(id, existingBrand);
            return Ok(existingBrand);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteBrand([FromRoute] int id)
        {
            var message = await _brandRepo.DeleteBrandAsync(id);
            return Ok(new { message });
        }
    }
}
