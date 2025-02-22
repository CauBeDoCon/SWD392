using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWD392.DTOs;
using SWD392.Models;
using SWD392.Repositories;

namespace SWD392.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartProductsController : ControllerBase
    {
        private readonly ICartProductRepository _cartProductRepo;

        public CartProductsController(ICartProductRepository repo)
        {
            _cartProductRepo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCartProducts([FromQuery] int? pageNumber, [FromQuery] int? pageSize)
        {
            int currentPage = pageNumber ?? 1;
            int currentSize = pageSize ?? 10;

            var result = await _cartProductRepo.GetAllCartProductsAsync(currentPage, currentSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCartProductById(int id)
        {
            try
            {
                var cartProduct = await _cartProductRepo.GetCartProductsAsync(id);
                return Ok(cartProduct);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Đã có lỗi xảy ra." });
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddNewCartProduct([FromBody] UpdateCartProductDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var model = new CartProductModel
            {
                Quantity = dto.Quantity,
                CartId = dto.CartId,
                ProductId = dto.ProductId
            };

            var newCartProductId = await _cartProductRepo.AddCartProductAsync(model);
            var cartProduct = await _cartProductRepo.GetCartProductsAsync(newCartProductId);
            return cartProduct == null ? NotFound() : Ok(cartProduct);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateCartProduct(int id, [FromBody] UpdateCartProductDto dto)
        {
            if (dto == null)
            {
                return BadRequest(new { message = "Dữ liệu không hợp lệ." });
            }

            try
            {
                var existingCartProduct = await _cartProductRepo.GetCartProductsAsync(id);
                existingCartProduct.Quantity = dto.Quantity;
                existingCartProduct.CartId = dto.CartId;
                existingCartProduct.ProductId = dto.ProductId;

                await _cartProductRepo.UpdateCartProductAsync(id, existingCartProduct);
                return Ok(existingCartProduct);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Đã có lỗi xảy ra." });
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteCartProduct([FromRoute] int id)
        {
            try
            {
                var message = await _cartProductRepo.DeleteCartProductAsync(id);
                return Ok(new { message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Đã có lỗi xảy ra." });
            }
        }
    }
}
