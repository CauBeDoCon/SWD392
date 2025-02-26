using SWD392.DTOs.Pagination;
using SWD392.Models;

namespace SWD392.Repositories
{
    public interface IOrderRepository
    {
        Task<PagedResult<OrderModel>> GetAllOrdersAsync(int pageNumber, int pageSize);
        public Task<OrderModel> GetOrdersAsync(int id);

        public Task<int> AddOrderAsync(OrderModel model);

        public Task UpdateOrderAsync(int id, OrderModel model);
        public Task<string> DeleteOrderAsync(int id);
        // Phương thức mới cho quy trình đặt hàng đầy đủ
        Task<bool> PlaceOrderAsync(int cartId, string userId);
    }
}
