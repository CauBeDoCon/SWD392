using SWD392.Models;

namespace SWD392.Repositories
{
    public interface IImageRepository
    {
        public Task<List<ImageModel>> GetAllImagesAsync();
        public Task<ImageModel> GetImagesAsync(int id);

        public Task<int> AddImageAsync(ImageModel model);

        public Task UpdateImageAsync(int id, ImageModel model);
        public Task DeleteImageAsync(int id);
    }
}
