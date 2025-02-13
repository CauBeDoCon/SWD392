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
        public async Task<IActionResult> GetAllPackagings()
        {
            return Ok(await _packagingRepo.GetAllPackagingsAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPackagingById(int id)
        {
            var packaging = await _packagingRepo.GetPackagingsAsync(id);
            return packaging == null ? NotFound() : Ok(packaging);
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
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var existingPackaging = await _packagingRepo.GetPackagingsAsync(id);
            if (existingPackaging == null)
            {
                return NotFound($"Không tìm thấy quy cách có ID = {id}");
            }

            existingPackaging.Name = dto.Name;

            await _packagingRepo.UpdatePackagingAsync(id, existingPackaging);
            return Ok(existingPackaging);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeletePackaging([FromRoute] int id)
        {
            await _packagingRepo.DeletePackagingAsync(id);
            return Ok();
        }
    }
}
