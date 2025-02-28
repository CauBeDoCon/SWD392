using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWD392.DB;
using SWD392.DTOs.Pagination;
using SWD392.Models;
using SWD392.Repositories;

namespace SWD392.Repositories
{
    public class RecommendProductRepository : IRecommendProductRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public RecommendProductRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<int> AddRecommendProductAsync(RecommendProductModel model)
        {
            var newRecommendProduct = _mapper.Map<RecommendProduct>(model);
            _context.RecommendProducts!.Add(newRecommendProduct);
            await _context.SaveChangesAsync();
            return newRecommendProduct.Id;
        }

        public async Task<string> DeleteRecommendProductAsync(int id)
        {
            var deleteRecommendProduct = await _context.RecommendProducts!.FindAsync(id);

            if (deleteRecommendProduct == null)
            {
                throw new KeyNotFoundException($"Sản phẩm đề xuất với ID {id} không tìm thấy.");
            }

            _context.RecommendProducts.Remove(deleteRecommendProduct);
            await _context.SaveChangesAsync();

            return $"Sản phẩm đề xuất với ID {id} đã xoá thành công.";
        }

        public async Task<PagedResult<RecommendProductModel>> GetAllRecommendProductsAsync(int pageNumber, int pageSize)
        {
            int totalCount = await _context.RecommendProducts!.CountAsync();

            var RecommendProducts = await _context.RecommendProducts!
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var mappedData = _mapper.Map<List<RecommendProductModel>>(RecommendProducts);

            return new PagedResult<RecommendProductModel>
            {
                Items = mappedData,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<RecommendProductModel> GetRecommendProductsAsync(int id)
        {
            var RecommendProducts = await _context.RecommendProducts.FindAsync(id);
            return _mapper.Map<RecommendProductModel>(RecommendProducts);
        }

        public async Task UpdateRecommendProductAsync(int id, RecommendProductModel model)
        {
            if (id != model.Id)
            {
                throw new ArgumentException("ID không khớp giữa request và model.");
            }

            var existingEntity = await _context.RecommendProducts!.FindAsync(id);
            if (existingEntity == null)
            {
                throw new KeyNotFoundException($"Sản phẩm đề xuất với ID {id} không tìm thấy.");
            }

            _context.Entry(existingEntity).State = EntityState.Detached;

            var updateRecommendProduct = _mapper.Map<RecommendProduct>(model);

            _context.RecommendProducts.Attach(updateRecommendProduct);
            _context.Entry(updateRecommendProduct).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }
    }
}
