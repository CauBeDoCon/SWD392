using SWD392.DB;
using SWD392.DTOs;
using SWD392.DTOs.Pagination;
using SWD392.Enums;
using SWD392.Models;

namespace SWD392.Repositories
{
    public interface IProductRepository
    {
        Task<PagedResult<ProductModel>> GetAllProductsAsync(int pageNumber, int pageSize);
        public Task<Models.ProductModel> GetProductByIdAsync(int id);

        public Task<int> AddProductAsync(Models.ProductModel model);

        public Task UpdateProductAsync(int id, Models.ProductModel model);
        public Task<string> DeleteProductAsync(int id);
        public Task<Product> GetMostProductBasedOnSkinTypeAsync(SkinType skinType, int? categoryId);
        public  Task<List<ProductwithImageDto>> GetProductsWithImagesByResultQuizId(int resultQuizId);
    }
}
