using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SWD392.DB;
using SWD392.DTOs;
using SWD392.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

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

        public async Task<ResponseMessage<ReviewModel>> GetReviewById(int reviewId)
        {
            var reviewEntity = await _context.Reviews
                .Include(r => r.OrderDetail)
                .Include(r => r.User) // Load thông tin User
                .FirstOrDefaultAsync(r => r.Id == reviewId);

            if (reviewEntity == null)
                return new ResponseMessage<ReviewModel>(false, "Review không tồn tại.");

            var reviewModel = new ReviewModel
            {
                Id = reviewEntity.Id,
                Rating = reviewEntity.Rating,
                Content = reviewEntity.Content,
                ReviewDate = reviewEntity.ReviewDate,
                OrderDetailId = reviewEntity.OrderDetailId,
                UserId = reviewEntity.UserId,

                // Lấy thông tin User
                UserName = reviewEntity.User.UserName,
                Avatar = reviewEntity.User.Avatar
            };

            return new ResponseMessage<ReviewModel>(true, "Lấy review thành công.", reviewModel);
        }


        public async Task<ResponseMessage<IEnumerable<ReviewModel>>> GetReviewsByProduct(int productId)
        {
            var reviews = await _context.Reviews
                .Include(r => r.OrderDetail)
                .Include(r => r.User) // Thêm Include để lấy thông tin User
                .Where(r => r.OrderDetail.ProductId == productId)
                .Select(r => new ReviewModel
                {
                    Id = r.Id,
                    Rating = r.Rating,
                    Content = r.Content,
                    ReviewDate = r.ReviewDate,
                    OrderDetailId = r.OrderDetailId,
                    UserId = r.UserId,
                    UserName = r.User.UserName, // Lấy UserName từ bảng User
                    Avatar = r.User.Avatar // Lấy Avatar từ bảng User
                })
                .ToListAsync();

            if (!reviews.Any())
                return new ResponseMessage<IEnumerable<ReviewModel>>(false, "Không có review nào cho sản phẩm này.");

            return new ResponseMessage<IEnumerable<ReviewModel>>(true, "Lấy danh sách review thành công.", reviews);
        }


        public async Task<ResponseMessage<int>> CreateReviewAsync(int orderDetailId, ReviewDTO dto, string currentUserId)
        {
            var orderDetail = await _context.OrderDetails
                .Include(od => od.Order)
                .FirstOrDefaultAsync(od => od.Id == orderDetailId);

            if (orderDetail == null || orderDetail.Order.UserId != currentUserId)
            {
                return new ResponseMessage<int>(false, "Bạn chưa mua sản phẩm này nên không thể review.");
            }

            // 🔥 Kiểm tra xem review đã tồn tại chưa
            var existingReview = await _context.Reviews
                .FirstOrDefaultAsync(r => r.OrderDetailId == orderDetailId);

            if (existingReview != null)
            {
                return new ResponseMessage<int>(false, "Bạn đã review sản phẩm này rồi.");
            }

            var newReview = new Review
            {
                UserId = currentUserId,
                Rating = dto.Rating,
                Content = dto.Content,
                OrderDetailId = orderDetailId,  // Dùng orderDetailId từ URL
                ReviewDate = DateTime.UtcNow
            };

            _context.Reviews.Add(newReview);
            await _context.SaveChangesAsync();

            return new ResponseMessage<int>(true, "Thêm review thành công.", newReview.Id);
        }


        public async Task<ResponseMessage<bool>> UpdateReviewAsync(int reviewId, UpdateReviewDTO dto, string currentUserId)
        {
            var existingReview = await _context.Reviews.FindAsync(reviewId);
            if (existingReview == null)
            {
                return new ResponseMessage<bool>(false, "Review không tồn tại.");
            }

            if (existingReview.UserId != currentUserId)
            {
                return new ResponseMessage<bool>(false, "Bạn không có quyền sửa review này.");
            }

            existingReview.Rating = dto.Rating;
            existingReview.Content = dto.Content;

            await _context.SaveChangesAsync();
            return new ResponseMessage<bool>(true, "Cập nhật review thành công.", true);
        }

        public async Task<ResponseMessage<bool>> DeleteReviewAsync(int reviewId, string currentUserId)
        {
            var existingReview = await _context.Reviews.FindAsync(reviewId);
            if (existingReview == null)
            {
                return new ResponseMessage<bool>(false, "Review không tồn tại.");
            }

            if (existingReview.UserId != currentUserId)
            {
                return new ResponseMessage<bool>(false, "Bạn không có quyền xoá review này.");
            }

            _context.Reviews.Remove(existingReview);
            await _context.SaveChangesAsync();
            return new ResponseMessage<bool>(true, "Xóa review thành công.", true);
        }
    }
}
