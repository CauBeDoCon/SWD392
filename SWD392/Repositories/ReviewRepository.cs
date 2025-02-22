using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWD392.DB;
using SWD392.DTOs.Pagination;
using SWD392.Models;
using SWD392.Repositories;

namespace SWD392.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ReviewRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<int> AddReviewAsync(ReviewModel model)
        {
            var newReview = _mapper.Map<Review>(model);
            _context.reviews!.Add(newReview);
            await _context.SaveChangesAsync();
            return newReview.Id;
        }

        public async Task<string> DeleteReviewAsync(int id)
        {
            var deleteReview = await _context.reviews!.FindAsync(id);

            if (deleteReview == null)
            {
                throw new KeyNotFoundException($"Đánh giá với ID {id} không tìm thấy.");
            }

            _context.reviews.Remove(deleteReview);
            await _context.SaveChangesAsync();

            return $"Đánh giá với ID {id} đã xoá thành công.";
        }

        public async Task<PagedResult<ReviewModel>> GetAllReviewsAsync(int pageNumber, int pageSize)
        {
            int totalCount = await _context.reviews!.CountAsync();

            var reviews = await _context.reviews!
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var mappedData = _mapper.Map<List<ReviewModel>>(reviews);

            return new PagedResult<ReviewModel>
            {
                Items = mappedData,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<ReviewModel> GetReviewsAsync(int id)
        {
            var review = await _context.reviews.FindAsync(id);

            if (review == null)
            {
                throw new KeyNotFoundException($"Đánh giá với ID {id} không tìm thấy.");
            }

            return _mapper.Map<ReviewModel>(review);
        }

        public async Task UpdateReviewAsync(int id, ReviewModel model)
        {
            if (id != model.Id)
            {
                throw new ArgumentException("ID không khớp giữa request và model.");
            }

            var existingEntity = await _context.reviews!.FindAsync(id);
            if (existingEntity == null)
            {
                throw new KeyNotFoundException($"Đánh giá với ID {id} không tìm thấy.");
            }

            _context.Entry(existingEntity).State = EntityState.Detached;

            var updateReview = _mapper.Map<Review>(model);

            _context.reviews.Attach(updateReview);
            _context.Entry(updateReview).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }
    }
}
