using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWD392.DB;
using SWD392.Models;
using SWD392.Repositories;

namespace SWD392.Repositories
{
    public class BrandRepository : IBrandRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public BrandRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<int> AddBrandAsync(BrandModel model)
        {
            var newBrand = _mapper.Map<Brand>(model);
            _context.brands!.Add(newBrand);
            await _context.SaveChangesAsync();
            return newBrand.Id;
        }

        public async Task DeleteBrandAsync(int id)
        {
            var deleteSkin = _context.brands!.SingleOrDefault(s => s.Id == id);
            if (deleteSkin != null)
            {
                _context.brands!.Remove(deleteSkin);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<BrandModel>> GetAllBrandsAsync()
        {
            var brands = await _context.brands!.ToListAsync();
            return _mapper.Map<List<BrandModel>>(brands);
        }

        public async Task<BrandModel> GetBrandsAsync(int id)
        {
            var brands = await _context.brands.FindAsync(id);
            return _mapper.Map<BrandModel>(brands);
        }

        public async Task UpdateBrandAsync(int id, BrandModel model)
        {
            if (id == model.Id)
            {
                var updateBrand = _mapper.Map<Brand>(model);
                _context.brands!.Update(updateBrand);
                await _context.SaveChangesAsync();

            }
        }
    }
}
