using SWD392.DTOs.Pagination;
using SWD392.Models;

namespace SWD392.Repositories
{
    public interface IManufacturedCountryRepository
    {
        Task<PagedResult<ManufacturedCountryModel>> GetAllManufacturedCountriesAsync(int pageNumber, int pageSize);
        public Task<ManufacturedCountryModel> GetManufacturedCountriesAsync(int id);

        public Task<int> AddManufacturedCountryAsync(ManufacturedCountryModel model);

        public Task UpdateManufacturedCountryAsync(int id, ManufacturedCountryModel model);
        public Task<string> DeleteManufacturedCountryAsync(int id);
    }
}
