using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWD392.DB;
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

        public async Task DeleteSolutionAsync(int id)
        {
            var deleteSkin = _context.solutions!.SingleOrDefault(s => s.Id == id);
            if (deleteSkin != null)
            {
                _context.solutions!.Remove(deleteSkin);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<SolutionModel>> GetAllSolutionsAsync()
        {
            var solutions = await _context.solutions!.ToListAsync();
            return _mapper.Map<List<SolutionModel>>(solutions);
        }

        public async Task<SolutionModel> GetSolutionsAsync(int id)
        {
            var solutions = await _context.solutions.FindAsync(id);
            return _mapper.Map<SolutionModel>(solutions);
        }

        public async Task UpdateSolutionAsync(int id, SolutionModel model)
        {
            if (id == model.Id)
            {
                var updateSolution = _mapper.Map<Solution>(model);
                _context.solutions!.Update(updateSolution);
                await _context.SaveChangesAsync();

            }
        }
    }
}
