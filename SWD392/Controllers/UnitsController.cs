using Microsoft.AspNetCore.Authorization;
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
        public async Task<IActionResult> GetAllUnits()
        {
            return Ok(await _unitRepo.GetAllUnitsAsync());
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
                return NotFound($"Không tìm thấy đơn vị tính có ID = {id}");
            }

            existingUnit.Name = dto.Name;

            await _unitRepo.UpdateUnitAsync(id, existingUnit);
            return Ok(existingUnit);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteUnit([FromRoute] int id)
        {
            await _unitRepo.DeleteUnitAsync(id);
            return Ok();
        }
    }
}
