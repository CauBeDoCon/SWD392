using SWD392.Models;

namespace SWD392.Repositories
{
    public interface IPackagingRepository
    {
        public Task<List<PackagingModel>> GetAllPackagingsAsync();
        public Task<PackagingModel> GetPackagingsAsync(int id);

        public Task<int> AddPackagingAsync(PackagingModel model);

        public Task UpdatePackagingAsync(int id, PackagingModel model);
        public Task DeletePackagingAsync(int id);
    }
}
