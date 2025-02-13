using SWD392.Models;

namespace SWD392.Repositories
{
    public interface IManufacturerRepository
    {
        public Task<List<ManufacturerModel>> GetAllManufacturersAsync();
        public Task<ManufacturerModel> GetManufacturersAsync(int id);

        public Task<int> AddManufacturerAsync(ManufacturerModel model);

        public Task UpdateManufacturerAsync(int id, ManufacturerModel model);
        public Task DeleteManufacturerAsync(int id);
    }
}
