using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWD392.DB;
using SWD392.DTOs.Pagination;
using SWD392.Models;
using SWD392.Repositories;

namespace SWD392.Repositories
{
    public class SolutionRepository : ISolutionRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public SolutionRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<int> AddSolutionAsync(SolutionModel model)
        {
            var newSolution = _mapper.Map<Solution>(model);
            _context.solutions!.Add(newSolution);
            await _context.SaveChangesAsync();
            return newSolution.Id;
        }

        public async Task<string> DeleteSolutionAsync(int id)
        {
            var deleteSolution = await _context.solutions!.FindAsync(id);

            if (deleteSolution == null)
            {
                throw new KeyNotFoundException($"Danh mục với ID {id} không tìm thấy.");
            }

            _context.solutions.Remove(deleteSolution);
            await _context.SaveChangesAsync();

            return $"Danh mục với ID {id} đã xoá thành công.";
        }

        public async Task<PagedResult<SolutionModel>> GetAllSolutionsAsync(int pageNumber, int pageSize)
        {
            int totalCount = await _context.solutions!.CountAsync();

            var solutions = await _context.solutions!
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var mappedData = _mapper.Map<List<SolutionModel>>(solutions);

            return new PagedResult<SolutionModel>
            {
                Items = mappedData,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<SolutionModel> GetSolutionsAsync(int id)
        {
            var solutions = await _context.solutions.FindAsync(id);
            return _mapper.Map<SolutionModel>(solutions);
        }

        public async Task UpdateSolutionAsync(int id, SolutionModel model)
        {
            if (id != model.Id)
            {
                throw new ArgumentException("ID không khớp giữa request và model.");
            }

            var existingEntity = await _context.solutions!.FindAsync(id);
            if (existingEntity == null)
            {
                throw new KeyNotFoundException($"Danh mục với ID {id} không tìm thấy.");
            }

            _context.Entry(existingEntity).State = EntityState.Detached;

            var updateSolution = _mapper.Map<Solution>(model);

            _context.solutions.Attach(updateSolution);
            _context.Entry(updateSolution).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }
    }
}
