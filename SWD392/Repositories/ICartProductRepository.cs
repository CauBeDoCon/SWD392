using SWD392.DTOs.Pagination;
using SWD392.Models;

namespace SWD392.Repositories
{
    public interface ICartProductRepository
    {
        Task<PagedResult<CartProductModel>> GetAllCartProductsAsync(int pageNumber, int pageSize);
        public Task<CartProductModel> GetCartProductsAsync(int id);

        public Task<int> AddCartProductAsync(CartProductModel model);

        public Task UpdateCartProductAsync(int id, CartProductModel model);
        public Task<string> DeleteCartProductAsync(int id);
        public  Task<CartProductModel> CheckProductExistInCart(int cartid, int ProductId ,CartProductModel model);
    }
}
