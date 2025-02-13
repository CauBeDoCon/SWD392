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
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepo;

        public ProductsController(IProductRepository repo)
        {
            _productRepo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSkincares()
        {
            return Ok(await _productRepo.GetAllProductsAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productRepo.GetProductByIdAsync(id);
            return product == null ? NotFound() : Ok(product);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddNewProduct([FromBody] UpdateProductDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var model = new ProductModel
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Quantity = dto.Quantity,
                BrandId = dto.BrandId,
                PackagingId = dto.PackagingId,
                CategoryId = dto.CategoryId,
                BrandOriginId = dto.BrandOriginId,
                ManufacturerId = dto.ManufacturerId,
                ManufacturedCountryId = dto.ManufacturedCountryId,
                ProductDetailId = dto.ProductDetailId
            };

            var newProductId = await _productRepo.AddProductAsync(model);
            var skincare = await _productRepo.GetProductByIdAsync(newProductId);
            return skincare == null ? NotFound() : Ok(skincare);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var existingSkincare = await _productRepo.GetProductByIdAsync(id);
            if (existingSkincare == null)
            {
                return NotFound($"Không tìm thấy sản phẩm có ID = {id}");
            }

            existingSkincare.Name = dto.Name;
            existingSkincare.Description = dto.Description;
            existingSkincare.Price = dto.Price;
            existingSkincare.Quantity = dto.Quantity;
            existingSkincare.BrandId = dto.BrandId;
            existingSkincare.PackagingId = dto.PackagingId;
            existingSkincare.CategoryId = dto.CategoryId;
            existingSkincare.BrandOriginId = dto.BrandOriginId;
            existingSkincare.ManufacturerId = dto.ManufacturerId;
            existingSkincare.ManufacturedCountryId = dto.ManufacturedCountryId;
            existingSkincare.ProductDetailId = dto.ProductDetailId;

            await _productRepo.UpdateProductAsync(id, existingSkincare);
            return Ok(existingSkincare);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteProduct([FromRoute] int id)
        {
            await _productRepo.DeleteProductAsync(id);
            return Ok();
        }
    }
}
