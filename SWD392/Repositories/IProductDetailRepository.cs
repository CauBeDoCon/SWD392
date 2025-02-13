using SWD392.DTOs.Pagination;
using SWD392.Models;

namespace SWD392.Repositories
{
    public interface IProductDetailRepository
    {
        Task<PagedResult<ProductDetailModel>> GetAllProductDetailsAsync(int pageNumber, int pageSize);
        public Task<ProductDetailModel> GetProductDetailsAsync(int id);

        public Task<int> AddProductDetailAsync(ProductDetailModel model);

        public Task UpdateProductDetailAsync(int id, ProductDetailModel model);
        public Task<string> DeleteProductDetailAsync(int id);
    }
}
