using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWD392.DB;
using SWD392.DTOs.Pagination;
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

        public async Task<string> DeleteBrandAsync(int id)
        {
            var deleteBrand = await _context.brands!.FindAsync(id);

            if (deleteBrand == null)
            {
                throw new KeyNotFoundException($"Thương hiệu với ID {id} không tìm thấy.");
            }

            _context.brands.Remove(deleteBrand);
            await _context.SaveChangesAsync();

            return $"Thương hiệu với ID {id} đã xoá thành công.";
        }

        public async Task<PagedResult<BrandModel>> GetAllBrandsAsync(int pageNumber, int pageSize)
        {
            int totalCount = await _context.brands!.CountAsync();

            var brands = await _context.brands!
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var mappedData = _mapper.Map<List<BrandModel>>(brands);

            return new PagedResult<BrandModel>
            {
                Items = mappedData,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<BrandModel> GetBrandsAsync(int id)
        {
            var brands = await _context.brands.FindAsync(id);
            return _mapper.Map<BrandModel>(brands);
        }

        public async Task UpdateBrandAsync(int id, BrandModel model)
        {
            if (id != model.Id)
            {
                throw new ArgumentException("ID không khớp giữa request và model.");
            }

            var existingEntity = await _context.brands!.FindAsync(id);
            if (existingEntity == null)
            {
                throw new KeyNotFoundException($"Thương hiệu với ID {id} không tìm thấy.");
            }

            _context.Entry(existingEntity).State = EntityState.Detached;

            var updateBrand = _mapper.Map<Brand>(model);

            _context.brands.Attach(updateBrand);
            _context.Entry(updateBrand).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }
    }
}
