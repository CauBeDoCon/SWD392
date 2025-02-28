using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWD392.DB;
using SWD392.DTOs.Pagination;
using SWD392.Models;
using SWD392.Repositories;

namespace SWD392.Repositories
{
    public class ReturnRepository : IReturnRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ReturnRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<int> AddReturnAsync(ReturnModel model)
        {
            var newReturn = _mapper.Map<Return>(model);
            _context.Returns!.Add(newReturn);
            await _context.SaveChangesAsync();
            return newReturn.Id;
        }

        public async Task<string> DeleteReturnAsync(int id)
        {
            var deleteReturn = await _context.Returns!.FindAsync(id);

            if (deleteReturn == null)
            {
                throw new KeyNotFoundException($"Hoàn trả với ID {id} không tìm thấy.");
            }

            _context.Returns.Remove(deleteReturn);
            await _context.SaveChangesAsync();

            return $"Hoàn trả với ID {id} đã xoá thành công.";
        }

        public async Task<PagedResult<ReturnModel>> GetAllReturnsAsync(int pageNumber, int pageSize)
        {
            int totalCount = await _context.Returns!.CountAsync();

            var Returns = await _context.Returns!
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var mappedData = _mapper.Map<List<ReturnModel>>(Returns);

            return new PagedResult<ReturnModel>
            {
                Items = mappedData,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<ReturnModel> GetReturnsAsync(int id)
        {
            var Returns = await _context.Returns.FindAsync(id);
            return _mapper.Map<ReturnModel>(Returns);
        }

        public async Task UpdateReturnAsync(int id, ReturnModel model)
        {
            if (id != model.Id)
            {
                throw new ArgumentException("ID không khớp giữa request và model.");
            }

            var existingEntity = await _context.Returns!.FindAsync(id);
            if (existingEntity == null)
            {
                throw new KeyNotFoundException($"Hoàn trả với ID {id} không tìm thấy.");
            }

            _context.Entry(existingEntity).State = EntityState.Detached;

            var updateReturn = _mapper.Map<Return>(model);

            _context.Returns.Attach(updateReturn);
            _context.Entry(updateReturn).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }
    }
}
