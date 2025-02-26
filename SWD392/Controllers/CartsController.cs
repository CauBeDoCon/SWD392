using SWD392.DTOs;
using Microsoft.AspNetCore.Mvc;
using SWD392.Repositories;

[Route("api/cart")]
[ApiController]
public class CartController : ControllerBase
{
    private readonly ICartRepository _cartRepository;

    public CartController(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    [HttpGet("{cartId}")]
    public async Task<IActionResult> GetCartProducts(int cartId)
    {
        var cartProducts = await _cartRepository.GetCartProductsAsync(cartId);

        if (cartProducts == null || !cartProducts.Any())
        {
            return NotFound(new { message = "Cart is empty!" });
        }

        return Ok(cartProducts);
    }

    [HttpDelete("{cartId}/product/{productId}")]
    public async Task<IActionResult> RemoveProductFromCart(int cartId, int productId)
    {
        bool success = await _cartRepository.RemoveProductFromCartAsync(cartId, productId);
        if (!success)
        {
            return NotFound(new { message = "Product not found in cart." });
        }
        return Ok(new { message = "Product removed from cart successfully." });
    }
}
