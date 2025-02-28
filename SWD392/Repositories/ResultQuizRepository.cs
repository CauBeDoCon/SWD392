using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWD392.DB;
using SWD392.DTOs.Pagination;
using SWD392.Models;
using SWD392.Repositories;

namespace SWD392.Repositories
{
    public class ResultQuizRepository : IResultQuizRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ResultQuizRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<int> AddResultQuizAsync(ResultQuizModel model)
        {
            var newResultQuiz = _mapper.Map<ResultQuiz>(model);
            _context.ResultQuizzes!.Add(newResultQuiz);
            await _context.SaveChangesAsync();
            return newResultQuiz.Id;
        }

        public async Task<string> DeleteResultQuizAsync(int id)
        {
            var deleteResultQuiz = await _context.ResultQuizzes!.FindAsync(id);

            if (deleteResultQuiz == null)
            {
                throw new KeyNotFoundException($"Kết quả bài test với ID {id} không tìm thấy.");
            }

            _context.ResultQuizzes.Remove(deleteResultQuiz);
            await _context.SaveChangesAsync();

            return $"Kết quả bài test với ID {id} đã xoá thành công.";
        }

        public async Task<PagedResult<ResultQuizModel>> GetAllResultQuizzesAsync(int pageNumber, int pageSize)
        {
            int totalCount = await _context.ResultQuizzes!.CountAsync();

            var ResultQuizs = await _context.ResultQuizzes!
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var mappedData = _mapper.Map<List<ResultQuizModel>>(ResultQuizs);

            return new PagedResult<ResultQuizModel>
            {
                Items = mappedData,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<ResultQuizModel> GetResultQuizzesAsync(int id)
        {
            var ResultQuizs = await _context.ResultQuizzes.FindAsync(id);
            return _mapper.Map<ResultQuizModel>(ResultQuizs);
        }

        public async Task UpdateResultQuizAsync(int id, ResultQuizModel model)
        {
            if (id != model.Id)
            {
                throw new ArgumentException("ID không khớp giữa request và model.");
            }

            var existingEntity = await _context.ResultQuizzes!.FindAsync(id);
            if (existingEntity == null)
            {
                throw new KeyNotFoundException($"Kết quả bài test với ID {id} không tìm thấy.");
            }

            _context.Entry(existingEntity).State = EntityState.Detached;

            var updateResultQuiz = _mapper.Map<ResultQuiz>(model);

            _context.ResultQuizzes.Attach(updateResultQuiz);
            _context.Entry(updateResultQuiz).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }
    }
}
