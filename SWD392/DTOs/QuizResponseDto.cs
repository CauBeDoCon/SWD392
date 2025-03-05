using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SWD392.DB;

namespace SWD392.DTOs
{
    public class QuizResponseDto
    {
        public ResultQuiz Data { get; set; }
        public string Message { get; set; }
    }
}