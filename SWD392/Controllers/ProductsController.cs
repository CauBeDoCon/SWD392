﻿using Microsoft.AspNetCore.Authorization;
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
        public async Task<IActionResult> GetAllProducts([FromQuery] int? pageNumber, [FromQuery] int? pageSize)
        {
            int currentPage = pageNumber ?? 1;
            int currentSize = pageSize ?? 10;

            var result = await _productRepo.GetAllProductsAsync(currentPage, currentSize);
            return Ok(result);
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
                UnitId = dto.UnitId,
                BrandId = dto.BrandId,
                PackagingId = dto.PackagingId,
                CategoryId = dto.CategoryId,
                BrandOriginId = dto.BrandOriginId,
                ManufacturerId = dto.ManufacturerId,
                ManufacturedCountryId = dto.ManufacturedCountryId,
                ProductDetailId = dto.ProductDetailId
            };

            var newProductId = await _productRepo.AddProductAsync(model);
            var product = await _productRepo.GetProductByIdAsync(newProductId);
            return product == null ? NotFound() : Ok(product);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var existingProduct = await _productRepo.GetProductByIdAsync(id);
            if (existingProduct == null)
            {
                return NotFound($"Không tìm thấy sản phẩm có ID = {id}");
            }

            existingProduct.Name = dto.Name;
            existingProduct.Description = dto.Description;
            existingProduct.Price = dto.Price;
            existingProduct.Quantity = dto.Quantity;
            existingProduct.UnitId = dto.UnitId;
            existingProduct.BrandId = dto.BrandId;
            existingProduct.PackagingId = dto.PackagingId;
            existingProduct.CategoryId = dto.CategoryId;
            existingProduct.BrandOriginId = dto.BrandOriginId;
            existingProduct.ManufacturerId = dto.ManufacturerId;
            existingProduct.ManufacturedCountryId = dto.ManufacturedCountryId;
            existingProduct.ProductDetailId = dto.ProductDetailId;

            await _productRepo.UpdateProductAsync(id, existingProduct);
            return Ok(existingProduct);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteProduct([FromRoute] int id)
        {
            var message = await _productRepo.DeleteProductAsync(id);
            return Ok(new { message });
        }
    }
}
