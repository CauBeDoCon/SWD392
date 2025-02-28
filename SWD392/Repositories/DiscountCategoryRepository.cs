using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SWD392.DB;
using SWD392.DTOs;
using SWD392.DTOs.Pagination;

namespace SWD392.Repositories
{
    public class DiscountCategoryRepository : IDiscountCategoryRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DiscountCategoryRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<int> AddDiscountCategoryAsync(DiscountCategoryDto model)
        {
            var newDiscountCategory = _mapper.Map<DiscountCategory>(model);
            _context.discountCategories!.Add(newDiscountCategory);
            await _context.SaveChangesAsync();
            return newDiscountCategory.Id;
        }

        public async Task<string> DeleteDiscountCategoryAsync(int id)
        {
            var deleteDiscountCategory = await _context.discountCategories!.FindAsync(id);

            if (deleteDiscountCategory == null)
            {
                throw new KeyNotFoundException($"Danh mục với ID {id} không tìm thấy.");
            }

            _context.discountCategories.Remove(deleteDiscountCategory);
            await _context.SaveChangesAsync();

            return $"Danh mục với ID {id} đã xoá thành công.";
        }

        public async Task<PagedResult<DiscountCategoryDto>> GetAllDiscountCategorysAsync(int pageNumber, int pageSize)
        {
           int totalCount = await _context.discountCategories!.CountAsync();

            var DiscountCategorys = await _context.discountCategories!
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var mappedData = _mapper.Map<List<DiscountCategoryDto>>(DiscountCategorys);

            return new PagedResult<DiscountCategoryDto>
            {
                Items = mappedData,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<DiscountCategoryDto> GetDiscountCategorysAsync(int id)
        {
            var discountCategories = await _context.discountCategories.FindAsync(id);
            return _mapper.Map<DiscountCategoryDto>(discountCategories);
        }

        public async Task UpdateDiscountCategoryAsync(int id, DiscountCategoryDto model)
        {
            var existingEntity = await _context.discountCategories!.FindAsync(id);
            if (existingEntity == null)
            {
                throw new KeyNotFoundException($"Danh mục với ID {id} không tìm thấy.");
            }
            // ✅ Sử dụng AutoMapper để cập nhật dữ liệu trực tiếp
            _mapper.Map(model, existingEntity);

            // ✅ Đánh dấu entity đã chỉnh sửa để EF theo dõi
            _context.discountCategories.Update(existingEntity);
            await _context.SaveChangesAsync();
        }
    }
}
