using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWD392.DB;
using SWD392.DTOs.Pagination;
using SWD392.Models;
using SWD392.Repositories;

namespace SWD392.Repositories
{
    public class ShippingRepository : IShippingRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ShippingRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<int> AddShippingAsync(ShippingModel model)
        {
            var newShipping = _mapper.Map<Shipping>(model);
            _context.Shippings!.Add(newShipping);
            await _context.SaveChangesAsync();
            return newShipping.Id;
        }

        public async Task<string> DeleteShippingAsync(int id)
        {
            var deleteShipping = await _context.Shippings!.FindAsync(id);

            if (deleteShipping == null)
            {
                throw new KeyNotFoundException($"Vận chuyển với ID {id} không tìm thấy.");
            }

            _context.Shippings.Remove(deleteShipping);
            await _context.SaveChangesAsync();

            return $"Vận chuyển với ID {id} đã xoá thành công.";
        }

        public async Task<PagedResult<ShippingModel>> GetAllShippingsAsync(int pageNumber, int pageSize)
        {
            int totalCount = await _context.Shippings!.CountAsync();

            var Shippings = await _context.Shippings!
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var mappedData = _mapper.Map<List<ShippingModel>>(Shippings);

            return new PagedResult<ShippingModel>
            {
                Items = mappedData,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<ShippingModel> GetShippingsAsync(int id)
        {
            var Shippings = await _context.Shippings.FindAsync(id);
            return _mapper.Map<ShippingModel>(Shippings);
        }

        public async Task UpdateShippingAsync(int id, ShippingModel model)
        {
            if (id != model.Id)
            {
                throw new ArgumentException("ID không khớp giữa request và model.");
            }

            var existingEntity = await _context.Shippings!.FindAsync(id);
            if (existingEntity == null)
            {
                throw new KeyNotFoundException($"Vận chuyển với ID {id} không tìm thấy.");
            }

            _context.Entry(existingEntity).State = EntityState.Detached;

            var updateShipping = _mapper.Map<Shipping>(model);

            _context.Shippings.Attach(updateShipping);
            _context.Entry(updateShipping).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }
    }
}
