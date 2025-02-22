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
        public async Task<IActionResult> GetAllBrandOrigins([FromQuery] int? pageNumber, [FromQuery] int? pageSize)
        {
            // Nếu người dùng không truyền giá trị, sử dụng mặc định
            int currentPage = pageNumber ?? 1;  // Mặc định là 1
            int currentSize = pageSize ?? 10;   // Mặc định là 10

            var result = await _brandOriginRepo.GetAllBrandOriginsAsync(currentPage, currentSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBrandOriginById(int id)
        {
            try
            {
                var brandOrigin = await _brandOriginRepo.GetBrandOriginsAsync(id);
                return Ok(brandOrigin);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Đã có lỗi xảy ra." });
            }
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
                return BadRequest(new { message = "Dữ liệu không hợp lệ." });
            }

            try
            {
                var existingBrandOrigin = await _brandOriginRepo.GetBrandOriginsAsync(id);
                existingBrandOrigin.Name = dto.Name;

                await _brandOriginRepo.UpdateBrandOriginAsync(id, existingBrandOrigin);
                return Ok(existingBrandOrigin);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Đã có lỗi xảy ra." });
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteBrandOrigin([FromRoute] int id)
        {
            try
            {
                var message = await _brandOriginRepo.DeleteBrandOriginAsync(id);
                return Ok(new { message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Đã có lỗi xảy ra." });
            }
        }
    }
}
