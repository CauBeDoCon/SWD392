using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWD392.DTOs;
using SWD392.Models;
using SWD392.Repositories;

namespace SWD392.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackagingsController : ControllerBase
    {
        private readonly IPackagingRepository _packagingRepo;

        public PackagingsController(IPackagingRepository repo)
        {
            _packagingRepo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPackagings([FromQuery] int? pageNumber, [FromQuery] int? pageSize)
        {
            int currentPage = pageNumber ?? 1;
            int currentSize = pageSize ?? 10;

            var result = await _packagingRepo.GetAllPackagingsAsync(currentPage, currentSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPackagingById(int id)
        {
            try
            {
                var packaging = await _packagingRepo.GetPackagingsAsync(id);
                return Ok(packaging);
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
        public async Task<IActionResult> AddNewPackaging([FromBody] UpdatePackagingDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var model = new PackagingModel
            {
                Name = dto.Name
            };

            var newPackagingId = await _packagingRepo.AddPackagingAsync(model);
            var packaging = await _packagingRepo.GetPackagingsAsync(newPackagingId);
            return packaging == null ? NotFound() : Ok(packaging);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdatePackaging(int id, [FromBody] UpdatePackagingDto dto)
        {
            if (dto == null)
            {
                return BadRequest(new { message = "Dữ liệu không hợp lệ." });
            }

            try
            {
                var existingPackaging = await _packagingRepo.GetPackagingsAsync(id);
                existingPackaging.Name = dto.Name;

                await _packagingRepo.UpdatePackagingAsync(id, existingPackaging);
                return Ok(existingPackaging);
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
        public async Task<IActionResult> DeletePackaging([FromRoute] int id)
        {
            try
            {
                var message = await _packagingRepo.DeletePackagingAsync(id);
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
