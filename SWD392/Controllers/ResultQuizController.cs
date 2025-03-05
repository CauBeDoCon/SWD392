using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SWD392.DTOs;
using SWD392.Repositories;

namespace SWD392.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResultQuizController :ControllerBase
    {
        private readonly IResultQuizRepository _iResultQuizRepo;

        public ResultQuizController(IResultQuizRepository repo)
        {
            _iResultQuizRepo = repo;
        }
        [HttpPost]
        public async Task<IActionResult> AddResultQuiz([FromBody] QuizAnswerUser quizAnswerUser)
        {
            if (quizAnswerUser == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Không xác định được người dùng.");
            }
            var newResultQuizId = await _iResultQuizRepo.CreateResultQuiz(quizAnswerUser,userId);
            return Ok( newResultQuizId );
        }
    }
}