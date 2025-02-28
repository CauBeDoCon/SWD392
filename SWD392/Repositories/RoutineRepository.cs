using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWD392.DB;
using SWD392.DTOs.Pagination;
using SWD392.Models;
using SWD392.Repositories;

namespace SWD392.Repositories
{
    public class RoutineRepository : IRoutineRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public RoutineRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<int> AddRoutineAsync(RoutineModel model)
        {
            var newRoutine = _mapper.Map<Routine>(model);
            _context.Routines!.Add(newRoutine);
            await _context.SaveChangesAsync();
            return newRoutine.Id;
        }

        public async Task<string> DeleteRoutineAsync(int id)
        {
            var deleteRoutine = await _context.Routines!.FindAsync(id);

            if (deleteRoutine == null)
            {
                throw new KeyNotFoundException($"Thói quen với ID {id} không tìm thấy.");
            }

            _context.Routines.Remove(deleteRoutine);
            await _context.SaveChangesAsync();

            return $"Thói quen với ID {id} đã xoá thành công.";
        }

        public async Task<PagedResult<RoutineModel>> GetAllRoutinesAsync(int pageNumber, int pageSize)
        {
            int totalCount = await _context.Routines!.CountAsync();

            var Routines = await _context.Routines!
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var mappedData = _mapper.Map<List<RoutineModel>>(Routines);

            return new PagedResult<RoutineModel>
            {
                Items = mappedData,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<RoutineModel> GetRoutinesAsync(int id)
        {
            var Routines = await _context.Routines.FindAsync(id);
            return _mapper.Map<RoutineModel>(Routines);
        }

        public async Task UpdateRoutineAsync(int id, RoutineModel model)
        {
            if (id != model.Id)
            {
                throw new ArgumentException("ID không khớp giữa request và model.");
            }

            var existingEntity = await _context.Routines!.FindAsync(id);
            if (existingEntity == null)
            {
                throw new KeyNotFoundException($"Thói quen với ID {id} không tìm thấy.");
            }

            _context.Entry(existingEntity).State = EntityState.Detached;

            var updateRoutine = _mapper.Map<Routine>(model);

            _context.Routines.Attach(updateRoutine);
            _context.Entry(updateRoutine).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }
    }
}
