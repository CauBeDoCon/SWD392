using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SWD392.DB;
using SWD392.DTOs;
using SWD392.Enums;

namespace SWD392.Repositories
{
    public interface IRoutineStepRepository
    {
        public Task<int> AddRoutineStepAsync(RoutineStepDto dto,int RoutineId);
        public Task<List<RoutineStep>> GetRoutineStepByRouteID(int id);
        public  Task<List<RoutineStep>> GetRoutineStepByRouteIDAndBasic(int id);
        public  Task<List<RoutineStep>> GetRoutineStepByRouteIDAndAdvanced(int id);
        public  Task CreateRoutineStepsAsync(List<Routine> routines,SkinType skinType);
    }
}