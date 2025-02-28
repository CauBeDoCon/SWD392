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
    public class RecommendProductsController : ControllerBase
    {
        private readonly IRecommendProductRepository _recommendProductRepo;

        public RecommendProductsController(IRecommendProductRepository repo)
        {
            _recommendProductRepo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRecommendProducts([FromQuery] int? pageNumber, [FromQuery] int? pageSize)
        {
            int currentPage = pageNumber ?? 1;
            int currentSize = pageSize ?? 10;

            var result = await _recommendProductRepo.GetAllRecommendProductsAsync(currentPage, currentSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRecommendProductById(int id)
        {
            var RecommendProduct = await _recommendProductRepo.GetRecommendProductsAsync(id);
            return RecommendProduct == null ? NotFound() : Ok(RecommendProduct);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddNewRecommendProduct([FromBody] UpdateRecommendProductDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var model = new RecommendProductModel
            {
                ProductId = dto.ProductId,
                RecommendReason = dto.RecommendReason,
                RoutineId = dto.RoutineId
            };

            var newRecommendProductId = await _recommendProductRepo.AddRecommendProductAsync(model);
            var RecommendProduct = await _recommendProductRepo.GetRecommendProductsAsync(newRecommendProductId);
            return RecommendProduct == null ? NotFound() : Ok(RecommendProduct);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateRecommendProduct(int id, [FromBody] UpdateRecommendProductDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var existingRecommendProduct = await _recommendProductRepo.GetRecommendProductsAsync(id);
            if (existingRecommendProduct == null)
            {
                return NotFound($"Không tìm thấy sản phẩm đề xuất có ID = {id}");
            }

            existingRecommendProduct.ProductId = dto.ProductId;
            existingRecommendProduct.RecommendReason = dto.RecommendReason;
            existingRecommendProduct.RoutineId = dto.RoutineId;

            await _recommendProductRepo.UpdateRecommendProductAsync(id, existingRecommendProduct);
            return Ok(existingRecommendProduct);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteRecommendProduct([FromRoute] int id)
        {
            var message = await _recommendProductRepo.DeleteRecommendProductAsync(id);
            return Ok(new { message });
        }
    }
}
