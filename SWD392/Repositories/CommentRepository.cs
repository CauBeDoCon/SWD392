using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWD392.DB;
using SWD392.DTOs.Pagination;
using SWD392.Models;
using SWD392.Repositories;

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
        public async Task<int> AddCommentAsync(CommentModel model)
        {
            var newComment = _mapper.Map<Comment>(model);
            _context.Comments!.Add(newComment);
            await _context.SaveChangesAsync();
            return newComment.Id;
        }

        public async Task<string> DeleteCommentAsync(int id)
        {
            var deleteComment = await _context.Comments!.FindAsync(id);

            if (deleteComment == null)
            {
                throw new KeyNotFoundException($"Phản hồi với ID {id} không tìm thấy.");
            }

            _context.Comments.Remove(deleteComment);
            await _context.SaveChangesAsync();

            return $"Phản hồi với ID {id} đã xoá thành công.";
        }

        public async Task<PagedResult<CommentModel>> GetAllCommentsAsync(int pageNumber, int pageSize)
        {
            int totalCount = await _context.Comments!.CountAsync();

            var Comments = await _context.Comments!
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var mappedData = _mapper.Map<List<CommentModel>>(Comments);

            return new PagedResult<CommentModel>
            {
                Items = mappedData,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<CommentModel> GetCommentsAsync(int id)
        {
            var Comments = await _context.Comments.FindAsync(id);
            return _mapper.Map<CommentModel>(Comments);
        }

        public async Task UpdateCommentAsync(int id, CommentModel model)
        {
            if (id != model.Id)
            {
                throw new ArgumentException("ID không khớp giữa request và model.");
            }

            var existingEntity = await _context.Comments!.FindAsync(id);
            if (existingEntity == null)
            {
                throw new KeyNotFoundException($"Phản hồi với ID {id} không tìm thấy.");
            }

            _context.Entry(existingEntity).State = EntityState.Detached;

            var updateComment = _mapper.Map<Comment>(model);

            _context.Comments.Attach(updateComment);
            _context.Entry(updateComment).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }
    }
}
