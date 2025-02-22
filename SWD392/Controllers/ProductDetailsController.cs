using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWD392.DTOs;
using SWD392.Models;
using SWD392.Repositories;

namespace SWD392.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductDetailsController : ControllerBase
    {
        private readonly IProductDetailRepository _productDetailRepo;

        public ProductDetailsController(IProductDetailRepository repo)
        {
            _productDetailRepo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProductDetails([FromQuery] int? pageNumber, [FromQuery] int? pageSize)
        {
            int currentPage = pageNumber ?? 1;
            int currentSize = pageSize ?? 10;

            var result = await _productDetailRepo.GetAllProductDetailsAsync(currentPage, currentSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductDetailById(int id)
        {
            try
            {
                var productDetail = await _productDetailRepo.GetProductDetailsAsync(id);
                return Ok(productDetail);
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
        public async Task<IActionResult> AddNewProductDetail([FromBody] UpdateProductDetailDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var model = new ProductDetailModel
            {
                ProductDescription = dto.ProductDescription,
                Ingredient = dto.Ingredient,
                Effect = dto.Effect,
                HowToUse = dto.HowToUse,
                SideEffect = dto.SideEffect,
                Note = dto.Note,
                Preserve = dto.Preserve
            };

            var newProductDetailId = await _productDetailRepo.AddProductDetailAsync(model);
            var productDetail = await _productDetailRepo.GetProductDetailsAsync(newProductDetailId);
            return productDetail == null ? NotFound() : Ok(productDetail);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateProductDetail(int id, [FromBody] UpdateProductDetailDto dto)
        {
            if (dto == null)
            {
                return BadRequest(new { message = "Dữ liệu không hợp lệ." });
            }

            try
            {
                var existingProductDetail = await _productDetailRepo.GetProductDetailsAsync(id);
                existingProductDetail.ProductDescription = dto.ProductDescription;
                existingProductDetail.Ingredient = dto.Ingredient;
                existingProductDetail.Effect = dto.Effect;
                existingProductDetail.HowToUse = dto.HowToUse;
                existingProductDetail.SideEffect = dto.SideEffect;
                existingProductDetail.Note = dto.Note;
                existingProductDetail.Preserve = dto.Preserve;

                await _productDetailRepo.UpdateProductDetailAsync(id, existingProductDetail);
                return Ok(existingProductDetail);
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
        public async Task<IActionResult> DeleteProductDetail([FromRoute] int id)
        {
            try
            {
                var message = await _productDetailRepo.DeleteProductDetailAsync(id);
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
