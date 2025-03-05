using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SWD392.DB;
using SWD392.Enums;

namespace SWD392.Repositories
{
    public interface IRoutineRepository
    {
        public Task<List<Routine>> AddRoutineAsync(int resultQuizId, SkinType skinType);
    }
}