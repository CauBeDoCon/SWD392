using SWD392.DB;
using SWD392.DTOs;

namespace SWD392.Repositories
{
    public interface ICartRepository
    {
        Task<IEnumerable<CartProductDTO>> GetCartProductsAsync(int cartId);
        Task<decimal> TotalPriceInCartProduct(int cartId);
    }
}
