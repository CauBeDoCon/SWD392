using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWD392.DB;
using SWD392.DTOs.Pagination;
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

        public async Task<string> DeleteManufacturedCountryAsync(int id)
        {
            var deleteManufacturedCountry = await _context.manufacturedCountries!.FindAsync(id);

            if (deleteManufacturedCountry == null)
            {
                throw new KeyNotFoundException($"Nước sản xuất với ID {id} không tìm thấy.");
            }

            _context.manufacturedCountries.Remove(deleteManufacturedCountry);
            await _context.SaveChangesAsync();

            return $"Nước sản xuất với ID {id} đã xoá thành công.";
        }

        public async Task<PagedResult<ManufacturedCountryModel>> GetAllManufacturedCountriesAsync(int pageNumber, int pageSize)
        {
            int totalCount = await _context.manufacturedCountries!.CountAsync();

            var manufacturedCountries = await _context.manufacturedCountries!
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var mappedData = _mapper.Map<List<ManufacturedCountryModel>>(manufacturedCountries);

            return new PagedResult<ManufacturedCountryModel>
            {
                Items = mappedData,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<ManufacturedCountryModel> GetManufacturedCountriesAsync(int id)
        {
            var manufacturedCountry = await _context.manufacturedCountries.FindAsync(id);

            if (manufacturedCountry == null)
            {
                throw new KeyNotFoundException($"Nước sản xuất với ID {id} không tìm thấy.");
            }

            return _mapper.Map<ManufacturedCountryModel>(manufacturedCountry);
        }

        public async Task UpdateManufacturedCountryAsync(int id, ManufacturedCountryModel model)
        {
            if (id != model.Id)
            {
                throw new ArgumentException("ID không khớp giữa request và model.");
            }

            var existingEntity = await _context.manufacturedCountries!.FindAsync(id);
            if (existingEntity == null)
            {
                throw new KeyNotFoundException($"Nước sản xuất với ID {id} không tìm thấy.");
            }

            _context.Entry(existingEntity).State = EntityState.Detached;

            var updateManufacturedCountry = _mapper.Map<ManufacturedCountry>(model);

            _context.manufacturedCountries.Attach(updateManufacturedCountry);
            _context.Entry(updateManufacturedCountry).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }
    }
}
