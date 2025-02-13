using SWD392.DB;
using SWD392.Models;

namespace SWD392.Repositories
{
    public interface IProductRepository
    {
        public Task<List<Models.ProductModel>> GetAllProductsAsync();
        public Task<Models.ProductModel> GetProductByIdAsync(int id);

        public Task<int> AddProductAsync(Models.ProductModel model);

        public Task UpdateProductAsync(int id, Models.ProductModel model);
        public Task DeleteProductAsync(int id);
    }
}
