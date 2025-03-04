using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SWD392.DB;
using SWD392.DTOs;
using SWD392.Models;
using System.Security.Claims;

namespace SWD392.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ReviewRepository(ApplicationDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ReviewModel> GetReviewById(int reviewId)
        {
            var reviewEntity = await _context.Reviews
                .Include(r => r.OrderDetail)
                .FirstOrDefaultAsync(r => r.Id == reviewId);

            if (reviewEntity == null)
                return null;

            return _mapper.Map<ReviewModel>(reviewEntity);
        }

        public async Task<IEnumerable<ReviewModel>> GetReviewsByProduct(int productId)
        {
            var reviews = await _context.Reviews
                .Include(r => r.OrderDetail)
                .Where(r => r.OrderDetail.ProductId == productId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ReviewModel>>(reviews);
        }

        public async Task<int> CreateReviewAsync(ReviewDTO dto, string currentUserId)
        {
            var orderDetail = await _context.OrderDetails
                .Include(od => od.Order)
                .FirstOrDefaultAsync(od => od.Id == dto.OrderDetailId);

            if (orderDetail == null || orderDetail.Order.UserId != currentUserId)
            {
                throw new UnauthorizedAccessException("Bạn chưa mua sản phẩm này nên không thể review.");
            }

            var newReview = new Review
            {
                UserId = currentUserId,
                Rating = dto.Rating,
                Content = dto.Content,
                OrderDetailId = dto.OrderDetailId,
                ReviewDate = DateTime.UtcNow
            };

            _context.Reviews.Add(newReview);
            await _context.SaveChangesAsync();
            return newReview.Id;
        }

        public async Task UpdateReviewAsync(int reviewId, UpdateReviewDTO dto, string currentUserId)
        {
            var existingReview = await _context.Reviews.FindAsync(reviewId);
            if (existingReview == null)
            {
                throw new KeyNotFoundException("Review không tồn tại.");
            }

            if (existingReview.UserId != currentUserId)
            {
                throw new UnauthorizedAccessException("Bạn không có quyền sửa review này.");
            }

            existingReview.Rating = dto.Rating;
            existingReview.Content = dto.Content;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteReviewAsync(int reviewId, string currentUserId)
        {
            var existingReview = await _context.Reviews.FindAsync(reviewId);
            if (existingReview == null)
            {
                throw new KeyNotFoundException("Review không tồn tại.");
            }

            if (existingReview.UserId != currentUserId)
            {
                throw new UnauthorizedAccessException("Bạn không có quyền xoá review này.");
            }

            _context.Reviews.Remove(existingReview);
            await _context.SaveChangesAsync();
        }
    }
}
