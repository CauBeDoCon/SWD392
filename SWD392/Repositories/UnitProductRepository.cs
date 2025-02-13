using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWD392.DB;
using SWD392.Models;
using SWD392.Repositories;

namespace SWD392.Repositories
{
    public class UnitProductRepository : IUnitProductRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UnitProductRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<int> AddUnitProductAsync(UnitProductModel model)
        {
            var newUnitProduct = _mapper.Map<UnitProduct>(model);
            _context.unitProducts!.Add(newUnitProduct);
            await _context.SaveChangesAsync();
            return newUnitProduct.Id;
        }

        public async Task DeleteUnitProductAsync(int id)
        {
            var deleteSkin = _context.unitProducts!.SingleOrDefault(s => s.Id == id);
            if (deleteSkin != null)
            {
                _context.unitProducts!.Remove(deleteSkin);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<UnitProductModel>> GetAllUnitProductsAsync()
        {
            var unitProducts = await _context.unitProducts!.ToListAsync();
            return _mapper.Map<List<UnitProductModel>>(unitProducts);
        }

        public async Task<UnitProductModel> GetUnitProductsAsync(int id)
        {
            var unitProducts = await _context.unitProducts.FindAsync(id);
            return _mapper.Map<UnitProductModel>(unitProducts);
        }

        public async Task UpdateUnitProductAsync(int id, UnitProductModel model)
        {
            if (id == model.Id)
            {
                var updateUnitProduct = _mapper.Map<UnitProduct>(model);
                _context.unitProducts!.Update(updateUnitProduct);
                await _context.SaveChangesAsync();

            }
        }
    }
}
