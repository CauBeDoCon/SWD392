using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWD392.DB;
using SWD392.Models;
using SWD392.Repositories;

namespace SWD392.Repositories
{
    public class BrandOriginRepository : IBrandOriginRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public BrandOriginRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<int> AddBrandOriginAsync(BrandOriginModel model)
        {
            var newBrandOrigin = _mapper.Map<BrandOrigin>(model);
            _context.brandOrigins!.Add(newBrandOrigin);
            await _context.SaveChangesAsync();
            return newBrandOrigin.Id;
        }

        public async Task DeleteBrandOriginAsync(int id)
        {
            var deleteSkin = _context.brandOrigins!.SingleOrDefault(s => s.Id == id);
            if (deleteSkin != null)
            {
                _context.brandOrigins!.Remove(deleteSkin);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<BrandOriginModel>> GetAllBrandOriginsAsync()
        {
            var brandOrigins = await _context.brandOrigins!.ToListAsync();
            return _mapper.Map<List<BrandOriginModel>>(brandOrigins);
        }

        public async Task<BrandOriginModel> GetBrandOriginsAsync(int id)
        {
            var brandOrigins = await _context.brandOrigins.FindAsync(id);
            return _mapper.Map<BrandOriginModel>(brandOrigins);
        }

        public async Task UpdateBrandOriginAsync(int id, BrandOriginModel model)
        {
            if (id == model.Id)
            {
                var updateBrandOrigin = _mapper.Map<BrandOrigin>(model);
                _context.brandOrigins!.Update(updateBrandOrigin);
                await _context.SaveChangesAsync();

            }
        }
    }
}
