using SWD392.DB;
using SWD392.DTOs.Pagination;
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
        Task<PagedResult<ProductModel>> GetProductsByPriceUnder100000Async(int pageNumber, int pageSize);
        Task<PagedResult<ProductModel>> GetProductsByPriceOver200000Async(int pageNumber, int pageSize);
        Task<PagedResult<ProductModel>> GetProductsByPriceOver400000Async(int pageNumber, int pageSize);
        Task<PagedResult<ProductModel>> GetProductsByPriceOver500000Async(int pageNumber, int pageSize);

        Task<PagedResult<ProductModel>> GetProductsByBrandNameAsync(string brandName, int pageNumber, int pageSize);
        Task<PagedResult<ProductModel>> GetProductsByBrandOriginNameAsync(string brandOriginName, int pageNumber, int pageSize);
        Task<PagedResult<ProductModel>> GetProductsByManufacturedCountryNameAsync(string manufacturedCountryName, int pageNumber, int pageSize);


        Task<PagedResult<ProductModel>> GetProductsByCategoryNameAsync(string categoryName, int pageNumber, int pageSize);
        Task<PagedResult<ProductModel>> GetProductsBySolutionNameAsync(string solutionName, int pageNumber, int pageSize);

    }
}
