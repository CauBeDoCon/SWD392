using SWD392.DTOs.Pagination;
using SWD392.Models;

namespace SWD392.Repositories
{
    public interface IPackagingRepository
    {
        Task<PagedResult<PackagingModel>> GetAllPackagingsAsync(int pageNumber, int pageSize);
        public Task<PackagingModel> GetPackagingsAsync(int id);

        public Task<int> AddPackagingAsync(PackagingModel model);

        public Task UpdatePackagingAsync(int id, PackagingModel model);
        public Task<string> DeletePackagingAsync(int id);
    }
}
