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
}
