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
    public class ResultQuizsController : ControllerBase
    {
        private readonly IResultQuizRepository _resultQuizRepo;

        public ResultQuizsController(IResultQuizRepository repo)
        {
            _resultQuizRepo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllResultQuizs([FromQuery] int? pageNumber, [FromQuery] int? pageSize)
        {
            int currentPage = pageNumber ?? 1;
            int currentSize = pageSize ?? 10;

            var result = await _resultQuizRepo.GetAllResultQuizzesAsync(currentPage, currentSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetResultQuizById(int id)
        {
            var ResultQuiz = await _resultQuizRepo.GetResultQuizzesAsync(id);
            return ResultQuiz == null ? NotFound() : Ok(ResultQuiz);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddNewResultQuiz([FromBody] UpdateResultQuizDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var model = new ResultQuizModel
            {
                Quiz1 = dto.Quiz1,
                Quiz2 = dto.Quiz2,
                Quiz3 = dto.Quiz3,
                Quiz4 = dto.Quiz4,
                Quiz5 = dto.Quiz5,
                Quiz6 = dto.Quiz6,
                Result = dto.Result,
                SkinStatus = dto.SkinStatus,
                Quiz7 = dto.Quiz7,
                AcneStatus = dto.AcneStatus,
                UserId = dto.UserId
            };

            var newResultQuizId = await _resultQuizRepo.AddResultQuizAsync(model);
            var ResultQuiz = await _resultQuizRepo.GetResultQuizzesAsync(newResultQuizId);
            return ResultQuiz == null ? NotFound() : Ok(ResultQuiz);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateResultQuiz(int id, [FromBody] UpdateResultQuizDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var existingResultQuiz = await _resultQuizRepo.GetResultQuizzesAsync(id);
            if (existingResultQuiz == null)
            {
                return NotFound($"Không tìm thấy kết quả bài test có ID = {id}");
            }

            existingResultQuiz.Quiz1 = dto.Quiz1;
            existingResultQuiz.Quiz2 = dto.Quiz2;
            existingResultQuiz.Quiz3 = dto.Quiz3;
            existingResultQuiz.Quiz4 = dto.Quiz4;
            existingResultQuiz.Quiz5 = dto.Quiz5;
            existingResultQuiz.Quiz6 = dto.Quiz6;
            existingResultQuiz.Result = dto.Result;
            existingResultQuiz.SkinStatus = dto.SkinStatus;
            existingResultQuiz.Quiz7 = dto.Quiz7;
            existingResultQuiz.AcneStatus = dto.AcneStatus;
            existingResultQuiz.UserId = dto.UserId;

            await _resultQuizRepo.UpdateResultQuizAsync(id, existingResultQuiz);
            return Ok(existingResultQuiz);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteResultQuiz([FromRoute] int id)
        {
            var message = await _resultQuizRepo.DeleteResultQuizAsync(id);
            return Ok(new { message });
        }
    }
}
