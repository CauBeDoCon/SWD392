using SWD392.DTOs;
using Microsoft.EntityFrameworkCore;
using SWD392.DB;
using SWD392.Repositories;
using SWD392.Enums;

public class CartRepository : ICartRepository
{
    private readonly ApplicationDbContext _context;

    // BẮT BUỘC phải có constructor
    public CartRepository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IEnumerable<CartProductDTO>> GetCartProductsAsync(int cartId)
    {
        var result = await _context.cartProducts
            .Where(cp => cp.CartId == cartId)
            .Include(cp => cp.Product) // Lấy Product kèm theo
            .Select(cp => new CartProductDTO
            {
                Id = cp.Id,
                Quantity = cp.Quantity,
                Status = CartStatus.pending.ToString(),
                CartId = cp.CartId,
                // Lồng Product
                Product = new ProductCartDTO
                {
                    Name = cp.Product.Name,
                    Description = cp.Product.Description,
                    Price = cp.Product.Price,
                    UnitId = cp.Product.UnitId,
                    BrandId = cp.Product.BrandId,
                    PackagingId = cp.Product.PackagingId,
                    CategoryId = cp.Product.CategoryId,
                    BrandOriginId = cp.Product.BrandOriginId,
                    ManufacturerId = cp.Product.ManufacturerId,
                    ManufacturedCountryId = cp.Product.ManufacturedCountryId,
                    ProductDetailId = cp.Product.ProductDetailId
                }
            })
            .ToListAsync();

        return result;
    }
    public async Task<decimal> TotalPriceInCartProduct(int cartId)
    {
        var cartProducts = await GetCartProductsAsync(cartId);
        return Convert.ToDecimal(cartProducts.Sum(cp => cp.Product.Price * cp.Quantity));
    }


}
