using SWD392.DTOs.Pagination;
using SWD392.Models;

namespace SWD392.Repositories
{
    public interface IShippingMethodRepository
    {
        Task<PagedResult<ShippingMethodModel>> GetAllShippingMethodsAsync(int pageNumber, int pageSize);
        public Task<ShippingMethodModel> GetShippingMethodsAsync(int id);

        public Task<int> AddShippingMethodAsync(ShippingMethodModel model);

        public Task UpdateShippingMethodAsync(int id, ShippingMethodModel model);
        public Task<string> DeleteShippingMethodAsync(int id);
    }
}
