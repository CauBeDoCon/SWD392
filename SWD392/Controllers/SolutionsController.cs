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
    public class SolutionsController : ControllerBase
    {
        private readonly ISolutionRepository _solutionRepo;

        public SolutionsController(ISolutionRepository repo)
        {
            _solutionRepo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSolutions([FromQuery] int? pageNumber, [FromQuery] int? pageSize)
        {
            // Nếu người dùng không truyền giá trị, sử dụng mặc định
            int currentPage = pageNumber ?? 1;  // Mặc định là 1
            int currentSize = pageSize ?? 10;   // Mặc định là 10

            var result = await _solutionRepo.GetAllSolutionsAsync(currentPage, currentSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSolutionById(int id)
        {
            var solution = await _solutionRepo.GetSolutionsAsync(id);
            return solution == null ? NotFound() : Ok(solution);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddNewSolution([FromBody] UpdateSolutionDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var model = new SolutionModel
            {
                Name = dto.Name
            };

            var newSolutionId = await _solutionRepo.AddSolutionAsync(model);
            var solution = await _solutionRepo.GetSolutionsAsync(newSolutionId);
            return solution == null ? NotFound() : Ok(solution);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateSolution(int id, [FromBody] UpdateSolutionDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var existingSolution = await _solutionRepo.GetSolutionsAsync(id);
            if (existingSolution == null)
            {
                return NotFound($"Không tìm thấy danh mục có ID = {id}");
            }

            existingSolution.Name = dto.Name;

            await _solutionRepo.UpdateSolutionAsync(id, existingSolution);
            return Ok(existingSolution);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteSolution([FromRoute] int id)
        {
            var message = await _solutionRepo.DeleteSolutionAsync(id);
            return Ok(new { message });
        }
    }
}
