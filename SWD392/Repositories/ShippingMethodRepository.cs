using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWD392.DB;
using SWD392.DTOs.Pagination;
using SWD392.Models;
using SWD392.Repositories;

namespace SWD392.Repositories
{
    public class ShippingMethodRepository : IShippingMethodRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ShippingMethodRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<int> AddShippingMethodAsync(ShippingMethodModel model)
        {
            var newShippingMethod = _mapper.Map<ShippingMethod>(model);
            _context.ShippingMethods!.Add(newShippingMethod);
            await _context.SaveChangesAsync();
            return newShippingMethod.Id;
        }

        public async Task<string> DeleteShippingMethodAsync(int id)
        {
            var deleteShippingMethod = await _context.ShippingMethods!.FindAsync(id);

            if (deleteShippingMethod == null)
            {
                throw new KeyNotFoundException($"Phương thức vận chuyển với ID {id} không tìm thấy.");
            }

            _context.ShippingMethods.Remove(deleteShippingMethod);
            await _context.SaveChangesAsync();

            return $"Phương thức vận chuyển với ID {id} đã xoá thành công.";
        }

        public async Task<PagedResult<ShippingMethodModel>> GetAllShippingMethodsAsync(int pageNumber, int pageSize)
        {
            int totalCount = await _context.ShippingMethods!.CountAsync();

            var ShippingMethods = await _context.ShippingMethods!
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var mappedData = _mapper.Map<List<ShippingMethodModel>>(ShippingMethods);

            return new PagedResult<ShippingMethodModel>
            {
                Items = mappedData,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<ShippingMethodModel> GetShippingMethodsAsync(int id)
        {
            var ShippingMethods = await _context.ShippingMethods.FindAsync(id);
            return _mapper.Map<ShippingMethodModel>(ShippingMethods);
        }

        public async Task UpdateShippingMethodAsync(int id, ShippingMethodModel model)
        {
            if (id != model.Id)
            {
                throw new ArgumentException("ID không khớp giữa request và model.");
            }

            var existingEntity = await _context.ShippingMethods!.FindAsync(id);
            if (existingEntity == null)
            {
                throw new KeyNotFoundException($"Phương thức vận chuyển với ID {id} không tìm thấy.");
            }

            _context.Entry(existingEntity).State = EntityState.Detached;

            var updateShippingMethod = _mapper.Map<ShippingMethod>(model);

            _context.ShippingMethods.Attach(updateShippingMethod);
            _context.Entry(updateShippingMethod).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }
    }
}
