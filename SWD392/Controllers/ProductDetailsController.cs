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
        public async Task<IActionResult> GetAllProductDetails()
        {
            return Ok(await _productDetailRepo.GetAllProductDetailsAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductDetailById(int id)
        {
            var productDetail = await _productDetailRepo.GetProductDetailsAsync(id);
            return productDetail == null ? NotFound() : Ok(productDetail);
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
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var existingProductDetail = await _productDetailRepo.GetProductDetailsAsync(id);
            if (existingProductDetail == null)
            {
                return NotFound($"Không tìm thấy chi tiết sản phẩm có ID = {id}");
            }

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

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteProductDetail([FromRoute] int id)
        {
            await _productDetailRepo.DeleteProductDetailAsync(id);
            return Ok();
        }
    }
}
