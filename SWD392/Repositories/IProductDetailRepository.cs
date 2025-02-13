using SWD392.Models;

namespace SWD392.Repositories
{
    public interface IProductDetailRepository
    {
        public Task<List<ProductDetailModel>> GetAllProductDetailsAsync();
        public Task<ProductDetailModel> GetProductDetailsAsync(int id);

        public Task<int> AddProductDetailAsync(ProductDetailModel model);

        public Task UpdateProductDetailAsync(int id, ProductDetailModel model);
        public Task DeleteProductDetailAsync(int id);
    }
}
