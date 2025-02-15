using SWD392.DTOs.Pagination;
using SWD392.Models;

namespace SWD392.Repositories
{
    public interface IManufacturerRepository
    {
        Task<PagedResult<ManufacturerModel>> GetAllManufacturersAsync(int pageNumber, int pageSize);
        public Task<ManufacturerModel> GetManufacturersAsync(int id);

        public Task<int> AddManufacturerAsync(ManufacturerModel model);

        public Task UpdateManufacturerAsync(int id, ManufacturerModel model);
        public Task<string> DeleteManufacturerAsync(int id);
    }
}
