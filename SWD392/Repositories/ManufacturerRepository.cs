using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWD392.DB;
using SWD392.DTOs.Pagination;
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

        public async Task<string> DeleteManufacturerAsync(int id)
        {
            var deleteManufacturer = await _context.manufacturers!.FindAsync(id);

            if (deleteManufacturer == null)
            {
                throw new KeyNotFoundException($"Nhà sản xuất với ID {id} không tìm thấy.");
            }

            _context.manufacturers.Remove(deleteManufacturer);
            await _context.SaveChangesAsync();

            return $"Nhà sản xuất với ID {id} đã xoá thành công.";
        }

        public async Task<PagedResult<ManufacturerModel>> GetAllManufacturersAsync(int pageNumber, int pageSize)
        {
            int totalCount = await _context.manufacturers!.CountAsync();

            var manufacturers = await _context.manufacturers!
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var mappedData = _mapper.Map<List<ManufacturerModel>>(manufacturers);

            return new PagedResult<ManufacturerModel>
            {
                Items = mappedData,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<ManufacturerModel> GetManufacturersAsync(int id)
        {
            var manufacturers = await _context.manufacturers.FindAsync(id);
            return _mapper.Map<ManufacturerModel>(manufacturers);
        }

        public async Task UpdateManufacturerAsync(int id, ManufacturerModel model)
        {
            if (id != model.Id)
            {
                throw new ArgumentException("ID không khớp giữa request và model.");
            }

            var existingEntity = await _context.manufacturers!.FindAsync(id);
            if (existingEntity == null)
            {
                throw new KeyNotFoundException($"Nhà sản xuất với ID {id} không tìm thấy.");
            }

            _context.Entry(existingEntity).State = EntityState.Detached;

            var updateManufacturer = _mapper.Map<Manufacturer>(model);

            _context.manufacturers.Attach(updateManufacturer);
            _context.Entry(updateManufacturer).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }
    }
}
