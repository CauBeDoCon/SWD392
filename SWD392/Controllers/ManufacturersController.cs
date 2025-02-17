using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWD392.DTOs;
using SWD392.Models;
using SWD392.Repositories;

namespace SWD392.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManufacturersController : ControllerBase
    {
        private readonly IManufacturerRepository _manufacturerRepo;

        public ManufacturersController(IManufacturerRepository repo)
        {
            _manufacturerRepo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllManufacturers([FromQuery] int? pageNumber, [FromQuery] int? pageSize)
        {
            int currentPage = pageNumber ?? 1;
            int currentSize = pageSize ?? 10;

            var result = await _manufacturerRepo.GetAllManufacturersAsync(currentPage, currentSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetManufacturerById(int id)
        {
            var manufacturer = await _manufacturerRepo.GetManufacturersAsync(id);
            return manufacturer == null ? NotFound() : Ok(manufacturer);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddNewManufacturer([FromBody] UpdateManufacturerDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var model = new ManufacturerModel
            {
                Name = dto.Name
            };

            var newManufacturerId = await _manufacturerRepo.AddManufacturerAsync(model);
            var manufacturer = await _manufacturerRepo.GetManufacturersAsync(newManufacturerId);
            return manufacturer == null ? NotFound() : Ok(manufacturer);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateManufacturer(int id, [FromBody] UpdateManufacturerDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var existingManufacturer = await _manufacturerRepo.GetManufacturersAsync(id);
            if (existingManufacturer == null)
            {
                return NotFound($"Không tìm thấy nhà sản xuất có ID = {id}");
            }

            existingManufacturer.Name = dto.Name;

            await _manufacturerRepo.UpdateManufacturerAsync(id, existingManufacturer);
            return Ok(existingManufacturer);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteManufacturer([FromRoute] int id)
        {
            var message = await _manufacturerRepo.DeleteManufacturerAsync(id);
            return Ok(new { message });
        }
    }
}
