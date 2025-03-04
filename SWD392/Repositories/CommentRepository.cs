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

        public async Task<CommentModel> GetCommentByIdAsync(int commentId)
        {
            var commentEntity = await _context.Comments
                .Include(c => c.Review)
                .FirstOrDefaultAsync(c => c.Id == commentId);
            if (commentEntity == null)
                return null;
            return _mapper.Map<CommentModel>(commentEntity);
        }

        public async Task<CommentModel> GetCommentByReviewIdAsync(int reviewId)
        {
            var commentEntity = await _context.Comments
                .Include(c => c.Review)
                .FirstOrDefaultAsync(c => c.ReviewId == reviewId);
            if (commentEntity == null)
                return null;
            return _mapper.Map<CommentModel>(commentEntity);
        }

        public async Task<int> CreateCommentAsync(CommentDTO dto, string currentUserId)
        {
            var existingComment = await _context.Comments
                .FirstOrDefaultAsync(c => c.ReviewId == dto.ReviewId);
            if (existingComment != null)
                throw new Exception("Review đã có comment.");

            var commentEntity = new Comment
            {
                Content = dto.Content,
                ReviewId = dto.ReviewId,
                UserId = currentUserId,
                CommentDate = DateTime.UtcNow
            };

            _context.Comments.Add(commentEntity);
            await _context.SaveChangesAsync();
            return commentEntity.Id;
        }

        public async Task UpdateCommentAsync(int commentId, UpdateCommentDTO dto, string currentUserId)
        {
            var commentEntity = await _context.Comments.FirstOrDefaultAsync(c => c.Id == commentId);
            if (commentEntity == null)
                throw new KeyNotFoundException("Không tìm thấy comment.");
            if (commentEntity.UserId != currentUserId)
                throw new UnauthorizedAccessException("Bạn không có quyền cập nhật comment này.");

            commentEntity.Content = dto.Content;
            commentEntity.CommentDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteCommentAsync(int commentId, string currentUserId)
        {
            var commentEntity = await _context.Comments.FirstOrDefaultAsync(c => c.Id == commentId);
            if (commentEntity == null)
                throw new KeyNotFoundException("Không tìm thấy comment.");
            if (commentEntity.UserId != currentUserId)
                throw new UnauthorizedAccessException("Bạn không có quyền xóa comment này.");

            _context.Comments.Remove(commentEntity);
            await _context.SaveChangesAsync();
        }
    }
}
