using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWD392.DB;
using SWD392.DTOs.Pagination;
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

        public async Task<string> DeleteUnitProductAsync(int id)
        {
            var deleteUnitProduct = await _context.unitProducts!.FindAsync(id);

            if (deleteUnitProduct == null)
            {
                throw new KeyNotFoundException($"Đơn vị tính với ID {id} không tìm thấy.");
            }

            _context.unitProducts.Remove(deleteUnitProduct);
            await _context.SaveChangesAsync();

            return $"Đơn vị tính với ID {id} đã xoá thành công.";
        }

        public async Task<PagedResult<UnitProductModel>> GetAllUnitProductsAsync(int pageNumber, int pageSize)
        {
            int totalCount = await _context.unitProducts!.CountAsync();

            var unitProducts = await _context.unitProducts!
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var mappedData = _mapper.Map<List<UnitProductModel>>(unitProducts);

            return new PagedResult<UnitProductModel>
            {
                Items = mappedData,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<UnitProductModel> GetUnitProductsAsync(int id)
        {
            var unitProducts = await _context.unitProducts.FindAsync(id);
            return _mapper.Map<UnitProductModel>(unitProducts);
        }

        public async Task UpdateUnitProductAsync(int id, UnitProductModel model)
        {
            if (id != model.Id)
            {
                throw new ArgumentException("ID không khớp giữa request và model.");
            }

            var existingEntity = await _context.unitProducts!.FindAsync(id);
            if (existingEntity == null)
            {
                throw new KeyNotFoundException($"Đơn vị tính với ID {id} không tìm thấy.");
            }

            _context.Entry(existingEntity).State = EntityState.Detached;

            var updateUnitProduct = _mapper.Map<UnitProduct>(model);

            _context.unitProducts.Attach(updateUnitProduct);
            _context.Entry(updateUnitProduct).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }
    }
}
