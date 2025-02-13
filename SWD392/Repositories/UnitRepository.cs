using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWD392.DB;
using SWD392.Models;
using SWD392.Repositories;

namespace SWD392.Repositories
{
    public class UnitRepository : IUnitRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UnitRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<int> AddUnitAsync(UnitModel model)
        {
            var newUnit = _mapper.Map<Unit>(model);
            _context.units!.Add(newUnit);
            await _context.SaveChangesAsync();
            return newUnit.Id;
        }

        public async Task DeleteUnitAsync(int id)
        {
            var deleteSkin = _context.units!.SingleOrDefault(s => s.Id == id);
            if (deleteSkin != null)
            {
                _context.units!.Remove(deleteSkin);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<UnitModel>> GetAllUnitsAsync()
        {
            var units = await _context.units!.ToListAsync();
            return _mapper.Map<List<UnitModel>>(units);
        }

        public async Task<UnitModel> GetUnitsAsync(int id)
        {
            var units = await _context.units.FindAsync(id);
            return _mapper.Map<UnitModel>(units);
        }

        public async Task UpdateUnitAsync(int id, UnitModel model)
        {
            if (id == model.Id)
            {
                var updateUnit = _mapper.Map<Unit>(model);
                _context.units!.Update(updateUnit);
                await _context.SaveChangesAsync();

            }
        }
    }
}
