using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWD392.DB;
using SWD392.DTOs.Pagination;
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

        public async Task<string> DeleteUnitAsync(int id)
        {
            var deleteUnit = await _context.units!.FindAsync(id);

            if (deleteUnit == null)
            {
                throw new KeyNotFoundException($"Đơn vị với ID {id} không tìm thấy.");
            }

            _context.units.Remove(deleteUnit);
            await _context.SaveChangesAsync();

            return $"Đơn vị với ID {id} đã xoá thành công.";
        }

        public async Task<PagedResult<UnitModel>> GetAllUnitsAsync(int pageNumber, int pageSize)
        {
            int totalCount = await _context.units!.CountAsync();

            var units = await _context.units!
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var mappedData = _mapper.Map<List<UnitModel>>(units);

            return new PagedResult<UnitModel>
            {
                Items = mappedData,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<UnitModel> GetUnitsAsync(int id)
        {
            var unit = await _context.units.FindAsync(id);

            if (unit == null)
            {
                throw new KeyNotFoundException($"Đơn vị với ID {id} không tìm thấy.");
            }

            return _mapper.Map<UnitModel>(unit);
        }

        public async Task UpdateUnitAsync(int id, UnitModel model)
        {
            if (id != model.Id)
            {
                throw new ArgumentException("ID không khớp giữa request và model.");
            }

            var existingEntity = await _context.units!.FindAsync(id);
            if (existingEntity == null)
            {
                throw new KeyNotFoundException($"Đơn vị với ID {id} không tìm thấy.");
            }

            _context.Entry(existingEntity).State = EntityState.Detached;

            var updateUnit = _mapper.Map<Unit>(model);

            _context.units.Attach(updateUnit);
            _context.Entry(updateUnit).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }
    }
}
