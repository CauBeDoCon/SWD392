using SWD392.Models;

namespace SWD392.Repositories
{
    public interface ICategoryRepository
    {
        public Task<List<CategoryModel>> GetAllCategoriesAsync();
        public Task<CategoryModel> GetCategoriesAsync(int id);

        public Task<int> AddCategoryAsync(CategoryModel model);

        public Task UpdateCategoryAsync(int id, CategoryModel model);
        public Task DeleteCategoryAsync(int id);
    }
}
