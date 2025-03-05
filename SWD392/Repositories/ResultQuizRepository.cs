using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SWD392.DB;
using SWD392.DTOs;
using SWD392.Enums;

namespace SWD392.Repositories
{
    public class ResultQuizRepository :IResultQuizRepository
    {
         private readonly IMapper _mapper;
         private readonly ApplicationDbContext _context;
         
         private readonly IRoutineRepository _routineRepository;
         public ResultQuizRepository(IMapper mapper, ApplicationDbContext context, IRoutineRepository routineRepository )
         {
             _mapper = mapper;
             _context = context;
             _routineRepository = routineRepository;
           
         }
        public async Task<QuizResponseDto> CreateResultQuiz(QuizAnswerUser resultQuiz, string userId)
        {
            
            // var answerCount = Enum.GetValues<ResultQuizAnswer>()
            //               .ToDictionary(answer => answer, _ => 0);
            // Dictionary để đếm số lượng chọn của mỗi đáp án
            var answerCount = new Dictionary<ResultQuizAnswer, int>
            {
                { ResultQuizAnswer.A, 0 },
                { ResultQuizAnswer.B, 0 },
                { ResultQuizAnswer.C, 0 },
                { ResultQuizAnswer.D, 0 }
            };

            // Lặp qua tất cả các câu trả lời và tăng số lần xuất hiện
            var answers = new List<ResultQuizAnswer>
            {
                resultQuiz.Quiz1, resultQuiz.Quiz2, resultQuiz.Quiz3, 
                resultQuiz.Quiz4, resultQuiz.Quiz5, resultQuiz.Quiz6, 
            };

            foreach (var answer in answers)
            {
                answerCount[answer]++;
            }

            // Tìm đáp án có số lượng chọn nhiều nhất
            var mostChosenAnswer1 = answerCount.OrderByDescending(x => x.Value).First();
            var secondChosenAnswer = answerCount.OrderByDescending(x => x.Value).Skip(1).FirstOrDefault();
            

            var mostChosenAnswer = mostChosenAnswer1.Key;
            var skinStatus = mostChosenAnswer switch
            {
                ResultQuizAnswer.A => SkinType.DaThuong,
                ResultQuizAnswer.B => SkinType.DaDau,
                ResultQuizAnswer.C => SkinType.DaKho,
                ResultQuizAnswer.D => SkinType.DaHonHop,
                _ => SkinType.Default
            };
            var anceStatus = resultQuiz.QuizAnces == ResultQuizAnswer.A ? AnceStatus.Yes : AnceStatus.No;
            var result = new ResultQuiz{
                UserId = userId,
                SkinStatus = skinStatus,
                AnceStatus = anceStatus,
                CreateDate = DateTime.Now,
                Quiz1 =    resultQuiz.Quiz1,
                Quiz2 =    resultQuiz.Quiz2,
                Quiz3 =    resultQuiz.Quiz3,
                Quiz4 =    resultQuiz.Quiz4,
                Quiz5 =    resultQuiz.Quiz5,
                Quiz6 =    resultQuiz.Quiz6,
                QuizAnces = resultQuiz.QuizAnces,
                Result = (int)mostChosenAnswer,
            };
            
            //var newResultQuiz = _mapper.Map<ResultQuiz>(result);
            var result1 = await _context.ResultQuizzes.AddAsync(result);  // Add the new result to the database
            await _context.SaveChangesAsync();
            await _routineRepository.AddRoutineAsync(result1.Entity.ResultQuizId, result.SkinStatus);
            
            if (mostChosenAnswer1.Value == secondChosenAnswer.Value)
            {
                return new QuizResponseDto 
                {
                    Data = result,
                    Message = "Hãy liên hệ bác sĩ."
                };
            }
            return new QuizResponseDto 
            {
                Data = result,
                Message = "Success"
            };
        }

    }
}