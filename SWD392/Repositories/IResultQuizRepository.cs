using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SWD392.DB;
using SWD392.DTOs;

namespace SWD392.Repositories
{
    public interface IResultQuizRepository
    {
        Task<QuizResponseDto> CreateResultQuiz(QuizAnswerUser quizAnswerUser,string userId);
    }
}