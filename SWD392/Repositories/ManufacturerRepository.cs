using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWD392.DB;
using SWD392.Models;
using SWD392.Repositories;

namespace SWD392.Repositories
{
    public class ManufacturerRepository : IManufacturerRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ManufacturerRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<int> AddManufacturerAsync(ManufacturerModel model)
        {
            var newManufacturer = _mapper.Map<Manufacturer>(model);
            _context.manufacturers!.Add(newManufacturer);
            await _context.SaveChangesAsync();
            return newManufacturer.Id;
        }

        public async Task DeleteManufacturerAsync(int id)
        {
            var deleteSkin = _context.manufacturers!.SingleOrDefault(s => s.Id == id);
            if (deleteSkin != null)
            {
                _context.manufacturers!.Remove(deleteSkin);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<ManufacturerModel>> GetAllManufacturersAsync()
        {
            var manufacturers = await _context.manufacturers!.ToListAsync();
            return _mapper.Map<List<ManufacturerModel>>(manufacturers);
        }

        public async Task<ManufacturerModel> GetManufacturersAsync(int id)
        {
            var manufacturers = await _context.manufacturers.FindAsync(id);
            return _mapper.Map<ManufacturerModel>(manufacturers);
        }

        public async Task UpdateManufacturerAsync(int id, ManufacturerModel model)
        {
            if (id == model.Id)
            {
                var updateManufacturer = _mapper.Map<Manufacturer>(model);
                _context.manufacturers!.Update(updateManufacturer);
                await _context.SaveChangesAsync();

            }
        }
    }
}
