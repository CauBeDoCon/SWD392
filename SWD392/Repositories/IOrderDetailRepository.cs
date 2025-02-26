using SWD392.DTOs.Pagination;
using SWD392.Models;

namespace SWD392.Repositories
{
    public interface IOrderDetailRepository
    {
        Task<PagedResult<OrderDetailModel>> GetAllOrderDetailsAsync(int pageNumber, int pageSize);
        public Task<OrderDetailModel> GetOrderDetailsAsync(int id);

        public Task<int> AddOrderDetailAsync(OrderDetailModel model);

        public Task UpdateOrderDetailAsync(int id, OrderDetailModel model);
        public Task<string> DeleteOrderDetailAsync(int id);
    }
}
