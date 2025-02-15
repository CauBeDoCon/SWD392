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
    public class UnitsController : ControllerBase
    {
        private readonly IUnitRepository _unitRepo;

        public UnitsController(IUnitRepository repo)
        {
            _unitRepo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUnits([FromQuery] int? pageNumber, [FromQuery] int? pageSize)
        {
            int currentPage = pageNumber ?? 1;  // Mặc định là 1
            int currentSize = pageSize ?? 10;   // Mặc định là 10

            var result = await _unitRepo.GetAllUnitsAsync(currentPage, currentSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUnitById(int id)
        {
            var unit = await _unitRepo.GetUnitsAsync(id);
            return unit == null ? NotFound() : Ok(unit);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddNewUnit([FromBody] UpdateUnitDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var model = new UnitModel
            {
                Name = dto.Name
            };

            var newUnitId = await _unitRepo.AddUnitAsync(model);
            var unit = await _unitRepo.GetUnitsAsync(newUnitId);
            return unit == null ? NotFound() : Ok(unit);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateUnit(int id, [FromBody] UpdateUnitDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var existingUnit = await _unitRepo.GetUnitsAsync(id);
            if (existingUnit == null)
            {
                return NotFound($"Không tìm thấy đơn vị có ID = {id}");
            }

            existingUnit.Name = dto.Name;

            await _unitRepo.UpdateUnitAsync(id, existingUnit);
            return Ok(existingUnit);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteUnit([FromRoute] int id)
        {
            var message = await _unitRepo.DeleteUnitAsync(id);
            return Ok(new { message });
        }
    }
}
