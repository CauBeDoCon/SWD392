using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SWD392.DB;
using SWD392.DTOs;
using SWD392.Model;
using SWD392.Models;
using System;
using System.Threading.Tasks;

namespace SWD392.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CommentRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResponseMessage<CommentModel>> GetCommentByIdAsync(int commentId)
        {
            var commentEntity = await _context.Comments
                .Include(c => c.Review)
                .FirstOrDefaultAsync(c => c.Id == commentId);

            if (commentEntity == null)
                return new ResponseMessage<CommentModel>(false, "Không tìm thấy comment.");

            return new ResponseMessage<CommentModel>(true, "Lấy comment thành công.", _mapper.Map<CommentModel>(commentEntity));
        }

        public async Task<ResponseMessage<CommentModel>> GetCommentByReviewIdAsync(int reviewId)
        {
            var commentEntity = await _context.Comments
                .Include(c => c.Review)
                .FirstOrDefaultAsync(c => c.ReviewId == reviewId);

            if (commentEntity == null)
                return new ResponseMessage<CommentModel>(false, "Không có comment nào cho review này.");

            return new ResponseMessage<CommentModel>(true, "Lấy comment thành công.", _mapper.Map<CommentModel>(commentEntity));
        }

        public async Task<ResponseMessage<int>> CreateCommentAsync(CommentDTO dto, string currentUserId)
        {
            var existingComment = await _context.Comments
                .FirstOrDefaultAsync(c => c.ReviewId == dto.ReviewId);

            if (existingComment != null)
                return new ResponseMessage<int>(false, "Review đã có comment.");

            var commentEntity = new Comment
            {
                Content = dto.Content,
                ReviewId = dto.ReviewId,
                UserId = currentUserId,
                CommentDate = DateTime.UtcNow
            };

            _context.Comments.Add(commentEntity);
            await _context.SaveChangesAsync();

            return new ResponseMessage<int>(true, "Tạo comment thành công.", commentEntity.Id);
        }

        public async Task<ResponseMessage<bool>> UpdateCommentAsync(int commentId, UpdateCommentDTO dto, string currentUserId)
        {
            var commentEntity = await _context.Comments.FirstOrDefaultAsync(c => c.Id == commentId);
            if (commentEntity == null)
                return new ResponseMessage<bool>(false, "Không tìm thấy comment.");

            if (commentEntity.UserId != currentUserId)
                return new ResponseMessage<bool>(false, "Bạn không có quyền cập nhật comment này.");

            commentEntity.Content = dto.Content;
            commentEntity.CommentDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return new ResponseMessage<bool>(true, "Cập nhật comment thành công.", true);
        }

        public async Task<ResponseMessage<bool>> DeleteCommentAsync(int commentId, string currentUserId)
        {
            var commentEntity = await _context.Comments.FirstOrDefaultAsync(c => c.Id == commentId);
            if (commentEntity == null)
                return new ResponseMessage<bool>(false, "Không tìm thấy comment.");

            if (commentEntity.UserId != currentUserId)
                return new ResponseMessage<bool>(false, "Bạn không có quyền xóa comment này.");

            _context.Comments.Remove(commentEntity);
            await _context.SaveChangesAsync();

            return new ResponseMessage<bool>(true, "Xóa comment thành công.", true);
        }
    }
}
