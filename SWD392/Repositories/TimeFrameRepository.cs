using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWD392.DB;
using SWD392.DTOs.Pagination;
using SWD392.Models;
using SWD392.Repositories;

namespace SWD392.Repositories
{
    public class TimeFrameRepository : ITimeFrameRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TimeFrameRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<int> AddTimeFrameAsync(TimeFrameModel model)
        {
            var newTimeFrame = _mapper.Map<TimeFrame>(model);
            _context.TimeFrames!.Add(newTimeFrame);
            await _context.SaveChangesAsync();
            return newTimeFrame.Id;
        }

        public async Task<string> DeleteTimeFrameAsync(int id)
        {
            var deleteTimeFrame = await _context.TimeFrames!.FindAsync(id);

            if (deleteTimeFrame == null)
            {
                throw new KeyNotFoundException($"Khung giờ với ID {id} không tìm thấy.");
            }

            _context.TimeFrames.Remove(deleteTimeFrame);
            await _context.SaveChangesAsync();

            return $"Khung giờ với ID {id} đã xoá thành công.";
        }

        public async Task<PagedResult<TimeFrameModel>> GetAllTimeFramesAsync(int pageNumber, int pageSize)
        {
            int totalCount = await _context.TimeFrames!.CountAsync();

            var TimeFrames = await _context.TimeFrames!
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var mappedData = _mapper.Map<List<TimeFrameModel>>(TimeFrames);

            return new PagedResult<TimeFrameModel>
            {
                Items = mappedData,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<TimeFrameModel> GetTimeFramesAsync(int id)
        {
            var TimeFrames = await _context.TimeFrames.FindAsync(id);
            return _mapper.Map<TimeFrameModel>(TimeFrames);
        }

        public async Task UpdateTimeFrameAsync(int id, TimeFrameModel model)
        {
            if (id != model.Id)
            {
                throw new ArgumentException("ID không khớp giữa request và model.");
            }

            var existingEntity = await _context.TimeFrames!.FindAsync(id);
            if (existingEntity == null)
            {
                throw new KeyNotFoundException($"Khung giờ với ID {id} không tìm thấy.");
            }

            _context.Entry(existingEntity).State = EntityState.Detached;

            var updateTimeFrame = _mapper.Map<TimeFrame>(model);

            _context.TimeFrames.Attach(updateTimeFrame);
            _context.Entry(updateTimeFrame).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }
    }
}
