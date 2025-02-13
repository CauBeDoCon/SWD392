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
    public class BrandOriginsController : ControllerBase
    {
        private readonly IBrandOriginRepository _brandOriginRepo;

        public BrandOriginsController(IBrandOriginRepository repo)
        {
            _brandOriginRepo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBrandOrigins()
        {
            return Ok(await _brandOriginRepo.GetAllBrandOriginsAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBrandOriginById(int id)
        {
            var brandOrigin = await _brandOriginRepo.GetBrandOriginsAsync(id);
            return brandOrigin == null ? NotFound() : Ok(brandOrigin);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddNewBrandOrigin([FromBody] UpdateBrandOriginDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var model = new BrandOriginModel
            {
                Name = dto.Name
            };

            var newBrandOriginId = await _brandOriginRepo.AddBrandOriginAsync(model);
            var brandOrigin = await _brandOriginRepo.GetBrandOriginsAsync(newBrandOriginId);
            return brandOrigin == null ? NotFound() : Ok(brandOrigin);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateBrandOrigin(int id, [FromBody] UpdateBrandOriginDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var existingBrandOrigin = await _brandOriginRepo.GetBrandOriginsAsync(id);
            if (existingBrandOrigin == null)
            {
                return NotFound($"Không tìm thấy xuất xứ thương hiệu có ID = {id}");
            }

            existingBrandOrigin.Name = dto.Name;

            await _brandOriginRepo.UpdateBrandOriginAsync(id, existingBrandOrigin);
            return Ok(existingBrandOrigin);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteBrandOrigin([FromRoute] int id)
        {
            await _brandOriginRepo.DeleteBrandOriginAsync(id);
            return Ok();
        }
    }
}
