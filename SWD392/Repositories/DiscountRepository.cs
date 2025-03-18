using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SWD392.DB;
using SWD392.DTOs;
using SWD392.DTOs.Pagination;
using Microsoft.EntityFrameworkCore;

namespace SWD392.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DiscountRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<int> AddDiscountAsync(DiscountRequestDto model)
        {
            var isValidCategory = await _context.discountCategories
                .AnyAsync(x => x.Id == model.DiscountCategoryId); //any tra true/false , tra ve 1 phan tu / where tra ve list
            
            if (!isValidCategory)
            {
                throw new Exception("Discount category không tồn tại.");
            }

            var newDiscount = _mapper.Map<Discount>(model);
             newDiscount.discountStatus = Enums.DiscountStatus.valided; // Hoặc trạng thái mong muốn
            _context.discounts!.Add(newDiscount);
            await _context.SaveChangesAsync();
            return newDiscount.Id;
        }

        public async Task<string> DeleteDiscountAsync(int id)
        {
            var deleteDiscount = await _context.discounts!.FindAsync(id);

            if (deleteDiscount == null)
            {
                throw new KeyNotFoundException($"Thể loại với ID {id} không tìm thấy.");
            }

            _context.discounts.Remove(deleteDiscount);
            await _context.SaveChangesAsync();

            return $"Thể loại với ID {id} đã xoá thành công.";
        }

        public async Task<PagedResult<DiscountDto>> GetAllDiscountAsync(int pageNumber, int pageSize)
        {
            int totalCount = await _context.discounts!.CountAsync();

            var categories = await _context.discounts!
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var mappedData = _mapper.Map<List<DiscountDto>>(categories);

            return new PagedResult<DiscountDto>
            {
                Items = mappedData,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
        public async Task<List<DiscountDto>> GetDiscountALLAsync()
        {
            var discount = await _context.discounts.ToListAsync();
            return _mapper.Map<List<DiscountDto>>(discount);
        }


        public async Task<DiscountDto> GetDiscountAsync(int id)
        {
            var discounts = await _context.discounts.FindAsync(id);
            return _mapper.Map<DiscountDto>(discounts);
        }

        public async Task UpdateDiscountAsync(int id, DiscountDto model)
        {
            var existingEntity = await _context.discounts!.FindAsync(id);
            if (existingEntity == null)
            {
                throw new KeyNotFoundException($"Danh mục với ID {id} không tìm thấy.");
            }
            // ✅ Sử dụng AutoMapper để cập nhật dữ liệu trực tiếp
            _mapper.Map(model, existingEntity);

            // ✅ Đánh dấu entity đã chỉnh sửa để EF theo dõi
            _context.discounts.Update(existingEntity);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateDiscountStatusAsync(int id)
        {

            var existingEntity = await _context.discounts!.FindAsync(id);
           
            if (existingEntity == null)
            {
                throw new KeyNotFoundException($"Danh mục với ID {id} không tìm thấy.");
            }
           existingEntity.discountStatus = Enums.DiscountStatus.expired;

            // ✅ Đánh dấu entity đã chỉnh sửa để EF theo dõi
            _context.discounts.Update(existingEntity);
            await _context.SaveChangesAsync();
        }
    }
}