using SWD392.DTOs.Pagination;
using SWD392.Models;

namespace SWD392.Repositories
{
    public interface ICategoryRepository
    {
        Task<PagedResult<CategoryModel>> GetAllCategoriesAsync(int pageNumber, int pageSize);
        public Task<CategoryModel> GetCategoriesAsync(int id);

        public Task<int> AddCategoryAsync(CategoryModel model);

        public Task UpdateCategoryAsync(int id, CategoryModel model);
        public Task<string> DeleteCategoryAsync(int id);
    }
}
