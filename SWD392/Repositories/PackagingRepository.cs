using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWD392.DB;
using SWD392.DTOs.Pagination;
using SWD392.Models;
using SWD392.Repositories;

namespace SWD392.Repositories
{
    public class PackagingRepository : IPackagingRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public PackagingRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<int> AddPackagingAsync(PackagingModel model)
        {
            var newPackaging = _mapper.Map<Packaging>(model);
            _context.packagings!.Add(newPackaging);
            await _context.SaveChangesAsync();
            return newPackaging.Id;
        }

        public async Task<string> DeletePackagingAsync(int id)
        {
            var deletePackaging = await _context.packagings!.FindAsync(id);

            if (deletePackaging == null)
            {
                throw new KeyNotFoundException($"Quy cách với ID {id} không tìm thấy.");
            }

            _context.packagings.Remove(deletePackaging);
            await _context.SaveChangesAsync();

            return $"Quy cách với ID {id} đã xoá thành công.";
        }

        public async Task<PagedResult<PackagingModel>> GetAllPackagingsAsync(int pageNumber, int pageSize)
        {
            int totalCount = await _context.packagings!.CountAsync();

            var packagings = await _context.packagings!
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var mappedData = _mapper.Map<List<PackagingModel>>(packagings);

            return new PagedResult<PackagingModel>
            {
                Items = mappedData,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<PackagingModel> GetPackagingsAsync(int id)
        {
            var packagings = await _context.packagings.FindAsync(id);
            return _mapper.Map<PackagingModel>(packagings);
        }

        public async Task UpdatePackagingAsync(int id, PackagingModel model)
        {
            if (id != model.Id)
            {
                throw new ArgumentException("ID không khớp giữa request và model.");
            }

            var existingEntity = await _context.packagings!.FindAsync(id);
            if (existingEntity == null)
            {
                throw new KeyNotFoundException($"Quy cách với ID {id} không tìm thấy.");
            }

            _context.Entry(existingEntity).State = EntityState.Detached;

            var updatePackaging = _mapper.Map<Packaging>(model);

            _context.packagings.Attach(updatePackaging);
            _context.Entry(updatePackaging).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }
    }
}
