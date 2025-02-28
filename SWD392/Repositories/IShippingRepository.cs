using SWD392.DTOs.Pagination;
using SWD392.Models;

namespace SWD392.Repositories
{
    public interface IShippingRepository
    {
        Task<PagedResult<ShippingModel>> GetAllShippingsAsync(int pageNumber, int pageSize);
        public Task<ShippingModel> GetShippingsAsync(int id);

        public Task<int> AddShippingAsync(ShippingModel model);

        public Task UpdateShippingAsync(int id, ShippingModel model);
        public Task<string> DeleteShippingAsync(int id);
    }
}
