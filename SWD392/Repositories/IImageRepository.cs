
using SWD392.DB;
using SWD392.DTOs.Pagination;
using SWD392.Models;

namespace SWD392.Repositories
{
    public interface IImageRepository
    {
        Task<PagedResult<ImageModel>> GetAllImagesAsync(int pageNumber, int pageSize);
        public Task<List<Image>> GetImagesByProductID(int id);
        public Task<ImageModel> GetImagesAsync(int id);

        public Task<int> AddImageAsync(ImageModel model);

        public Task UpdateImageAsync(int id, ImageModel model);
        public Task<string> DeleteImageAsync(int id);
    }
}
