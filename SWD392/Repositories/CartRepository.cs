using SWD392.DTOs;
using Microsoft.EntityFrameworkCore;
using SWD392.DB;
using SWD392.Repositories;

public class CartRepository : ICartRepository
{
    private readonly ApplicationDbContext _context;

    public CartRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CartProductDTO>> GetCartProductsAsync(int cartId)
    {
        var result = await _context.cartProducts
            .Where(cp => cp.CartId == cartId)
            .Include(cp => cp.Product) // Bao gồm bảng Product
            .Select(cp => new CartProductDTO
            {
                Id = cp.Id,
                Quantity = cp.Quantity,
                Status = cp.Status,
                CartId = cp.CartId,
                ProductId = cp.ProductId,
                ProductName = cp.Product.Name, // Lấy từ bảng Product
                Price = cp.Product.Price // Lấy từ bảng Product
            })
            .ToListAsync();

        return result;
    }

    public async Task<bool> RemoveProductFromCartAsync(int cartId, int productId)
    {
        var cartProduct = await _context.cartProducts
            .FirstOrDefaultAsync(cp => cp.CartId == cartId && cp.ProductId == productId);

        if (cartProduct == null)
        {
            return false;
        }

        _context.cartProducts.Remove(cartProduct);
        await _context.SaveChangesAsync();
        return true;
    }
}
