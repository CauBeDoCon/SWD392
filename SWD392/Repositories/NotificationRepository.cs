using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWD392.DB;
using SWD392.DTOs.Pagination;
using SWD392.Models;
using SWD392.Repositories;

namespace SWD392.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public NotificationRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<int> AddNotificationAsync(NotificationModel model)
        {
            var newNotification = _mapper.Map<Notification>(model);
            _context.Notifications!.Add(newNotification);
            await _context.SaveChangesAsync();
            return newNotification.Id;
        }

        public async Task<string> DeleteNotificationAsync(int id)
        {
            var deleteNotification = await _context.Notifications!.FindAsync(id);

            if (deleteNotification == null)
            {
                throw new KeyNotFoundException($"Thông báo với ID {id} không tìm thấy.");
            }

            _context.Notifications.Remove(deleteNotification);
            await _context.SaveChangesAsync();

            return $"Thông báo với ID {id} đã xoá thành công.";
        }

        public async Task<PagedResult<NotificationModel>> GetAllNotificationsAsync(int pageNumber, int pageSize)
        {
            int totalCount = await _context.Notifications!.CountAsync();

            var Notifications = await _context.Notifications!
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var mappedData = _mapper.Map<List<NotificationModel>>(Notifications);

            return new PagedResult<NotificationModel>
            {
                Items = mappedData,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<NotificationModel> GetNotificationsAsync(int id)
        {
            var Notifications = await _context.Notifications.FindAsync(id);
            return _mapper.Map<NotificationModel>(Notifications);
        }

        public async Task UpdateNotificationAsync(int id, NotificationModel model)
        {
            if (id != model.Id)
            {
                throw new ArgumentException("ID không khớp giữa request và model.");
            }

            var existingEntity = await _context.Notifications!.FindAsync(id);
            if (existingEntity == null)
            {
                throw new KeyNotFoundException($"Thông báo với ID {id} không tìm thấy.");
            }

            _context.Entry(existingEntity).State = EntityState.Detached;

            var updateNotification = _mapper.Map<Notification>(model);

            _context.Notifications.Attach(updateNotification);
            _context.Entry(updateNotification).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }
    }
}
