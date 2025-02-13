using SWD392.Models;

namespace SWD392.Repositories
{
    public interface IManufacturedCountryRepository
    {
        public Task<List<ManufacturedCountryModel>> GetAllManufacturedCountriesAsync();
        public Task<ManufacturedCountryModel> GetManufacturedCountriesAsync(int id);

        public Task<int> AddManufacturedCountryAsync(ManufacturedCountryModel model);

        public Task UpdateManufacturedCountryAsync(int id, ManufacturedCountryModel model);
        public Task DeleteManufacturedCountryAsync(int id);
    }
}
