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
    public class RoutinesController : ControllerBase
    {
        private readonly IRoutineRepository _routineRepo;

        public RoutinesController(IRoutineRepository repo)
        {
            _routineRepo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoutines([FromQuery] int? pageNumber, [FromQuery] int? pageSize)
        {
            int currentPage = pageNumber ?? 1;
            int currentSize = pageSize ?? 10;

            var result = await _routineRepo.GetAllRoutinesAsync(currentPage, currentSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoutineById(int id)
        {
            var Routine = await _routineRepo.GetRoutinesAsync(id);
            return Routine == null ? NotFound() : Ok(Routine);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddNewRoutine([FromBody] UpdateRoutineDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var model = new RoutineModel
            {
                ResultQuizId = dto.ResultQuizId,
                StepOrder = dto.StepOrder,
                Instruction = dto.Instruction
            };

            var newRoutineId = await _routineRepo.AddRoutineAsync(model);
            var Routine = await _routineRepo.GetRoutinesAsync(newRoutineId);
            return Routine == null ? NotFound() : Ok(Routine);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateRoutine(int id, [FromBody] UpdateRoutineDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var existingRoutine = await _routineRepo.GetRoutinesAsync(id);
            if (existingRoutine == null)
            {
                return NotFound($"Không tìm thấy thói quen có ID = {id}");
            }

            existingRoutine.ResultQuizId = dto.ResultQuizId;
            existingRoutine.StepOrder = dto.StepOrder;
            existingRoutine.Instruction = dto.Instruction;

            await _routineRepo.UpdateRoutineAsync(id, existingRoutine);
            return Ok(existingRoutine);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteRoutine([FromRoute] int id)
        {
            var message = await _routineRepo.DeleteRoutineAsync(id);
            return Ok(new { message });
        }
    }
}
