    using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SWD392.DB;
using SWD392.Enums;

namespace SWD392.Repositories
{
    public class RoutineRepository :IRoutineRepository
    {
        public readonly ApplicationDbContext _context;
        public readonly IRoutineStepRepository _routineStepRepository;
        public RoutineRepository(ApplicationDbContext context, IRoutineStepRepository routineStepRepository)
        {
            _context = context;
            _routineStepRepository = routineStepRepository;
        }
        public async Task<List<Routine>> AddRoutineAsync(int resultQuizId, SkinType skinType)
        {
            var routines = new List<Routine>
            {
                new Routine
                {
                    ResultQuizId = resultQuizId,
                    RoutineCategory = Enums.RoutineCategoryType.BasicDay
                },
                new Routine
                {
                    ResultQuizId = resultQuizId,
                    RoutineCategory = Enums.RoutineCategoryType.BasicNight
                },
                new Routine
                {
                    ResultQuizId = resultQuizId,
                    RoutineCategory = Enums.RoutineCategoryType.AdvancedDay
                },
                new Routine
                {
                    ResultQuizId = resultQuizId,
                    RoutineCategory = Enums.RoutineCategoryType.AdvancedNight
                }
            };
            
            await _context.Routines.AddRangeAsync(routines);
            await _context.SaveChangesAsync();
            await _routineStepRepository.CreateRoutineStepsAsync(routines,skinType);
            return routines;
        }

    }
}