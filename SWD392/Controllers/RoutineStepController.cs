using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SWD392.Repositories;

namespace SWD392.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoutineStepController :ControllerBase
    {
        private readonly IRoutineStepRepository _routeStepRepository;

        public RoutineStepController(IRoutineStepRepository routeStepRepository)
        {
            _routeStepRepository = routeStepRepository;
        }

        [HttpGet("GetRouteStepsByUserIDAsync/{resultQuizId}")]
        public async Task<IActionResult> GetRouteStepsByUserIDAsync(int  resultQuizId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var routeSteps = await _routeStepRepository.GetRouteStepsByUserIDAsync(userId ,  resultQuizId);
            return Ok(routeSteps);
        }
    }
}