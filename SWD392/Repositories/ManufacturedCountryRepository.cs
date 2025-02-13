using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWD392.DB;
using SWD392.Models;
using SWD392.Repositories;

namespace SWD392.Repositories
{
    public class ManufacturedCountryRepository : IManufacturedCountryRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ManufacturedCountryRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<int> AddManufacturedCountryAsync(ManufacturedCountryModel model)
        {
            var newManufacturedCountry = _mapper.Map<ManufacturedCountry>(model);
            _context.manufacturedCountries!.Add(newManufacturedCountry);
            await _context.SaveChangesAsync();
            return newManufacturedCountry.Id;
        }

        public async Task DeleteManufacturedCountryAsync(int id)
        {
            var deleteSkin = _context.manufacturedCountries!.SingleOrDefault(s => s.Id == id);
            if (deleteSkin != null)
            {
                _context.manufacturedCountries!.Remove(deleteSkin);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<ManufacturedCountryModel>> GetAllManufacturedCountriesAsync()
        {
            var manufacturedCountries = await _context.manufacturedCountries!.ToListAsync();
            return _mapper.Map<List<ManufacturedCountryModel>>(manufacturedCountries);
        }

        public async Task<ManufacturedCountryModel> GetManufacturedCountriesAsync(int id)
        {
            var manufacturedCountries = await _context.manufacturedCountries.FindAsync(id);
            return _mapper.Map<ManufacturedCountryModel>(manufacturedCountries);
        }

        public async Task UpdateManufacturedCountryAsync(int id, ManufacturedCountryModel model)
        {
            if (id == model.Id)
            {
                var updateManufacturedCountry = _mapper.Map<ManufacturedCountry>(model);
                _context.manufacturedCountries!.Update(updateManufacturedCountry);
                await _context.SaveChangesAsync();

            }
        }
    }
}
