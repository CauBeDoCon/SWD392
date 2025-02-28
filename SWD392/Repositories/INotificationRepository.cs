using SWD392.DTOs.Pagination;
using SWD392.Models;

namespace SWD392.Repositories
{
    public interface INotificationRepository
    {
        Task<PagedResult<NotificationModel>> GetAllNotificationsAsync(int pageNumber, int pageSize);
        public Task<NotificationModel> GetNotificationsAsync(int id);

        public Task<int> AddNotificationAsync(NotificationModel model);

        public Task UpdateNotificationAsync(int id, NotificationModel model);
        public Task<string> DeleteNotificationAsync(int id);
    }
}
