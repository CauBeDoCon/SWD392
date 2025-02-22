using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWD392.DB;
using SWD392.DTOs.Pagination;
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

        public async Task<string> DeleteBrandOriginAsync(int id)
        {
            var deleteBrandOrigin = await _context.brandOrigins!.FindAsync(id);

            if (deleteBrandOrigin == null)
            {
                throw new KeyNotFoundException($"Xuất xứ thương hiệu với ID {id} không tìm thấy.");
            }

            _context.brandOrigins.Remove(deleteBrandOrigin);
            await _context.SaveChangesAsync();

            return $"Xuất xứ thương hiệu với ID {id} đã xoá thành công.";
        }

        public async Task<PagedResult<BrandOriginModel>> GetAllBrandOriginsAsync(int pageNumber, int pageSize)
        {
            int totalCount = await _context.brandOrigins!.CountAsync();

            var brandOrigins = await _context.brandOrigins!
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var mappedData = _mapper.Map<List<BrandOriginModel>>(brandOrigins);

            return new PagedResult<BrandOriginModel>
            {
                Items = mappedData,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<BrandOriginModel> GetBrandOriginsAsync(int id)
        {
            var brandOrigin = await _context.brandOrigins.FindAsync(id);

            if (brandOrigin == null)
            {
                throw new KeyNotFoundException($"Xuất xứ thương hiệu với ID {id} không tìm thấy.");
            }

            return _mapper.Map<BrandOriginModel>(brandOrigin);
        }

        public async Task UpdateBrandOriginAsync(int id, BrandOriginModel model)
        {
            if (id != model.Id)
            {
                throw new ArgumentException("ID không khớp giữa request và model.");
            }

            var existingEntity = await _context.brandOrigins!.FindAsync(id);
            if (existingEntity == null)
            {
                throw new KeyNotFoundException($"Xuất xứ thương hiệu với ID {id} không tìm thấy.");
            }

            _context.Entry(existingEntity).State = EntityState.Detached;

            var updateBrandOrigin = _mapper.Map<BrandOrigin>(model);

            _context.brandOrigins.Attach(updateBrandOrigin);
            _context.Entry(updateBrandOrigin).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }
    }
}
