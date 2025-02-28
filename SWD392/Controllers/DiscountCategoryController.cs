using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SWD392.DTOs;
using SWD392.Models;
using SWD392.Repositories;
namespace SWD392.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountCategoryController: ControllerBase
    {
        
        public readonly IDiscountCategoryRepository _discountCategoryRepo; 
        public DiscountCategoryController(IDiscountCategoryRepository repo)
        {
            _discountCategoryRepo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDiscountCategory([FromQuery] int? pageNumber, [FromQuery] int? pageSize)
        {
            // Nếu người dùng không truyền giá trị, sử dụng mặc định
            int currentPage = pageNumber ?? 1;  // Mặc định là 1
            int currentSize = pageSize ?? 10;   // Mặc định là 10

            var result = await _discountCategoryRepo.GetAllDiscountCategorysAsync(currentPage, currentSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetdiscountCategoryById(int id)
        {
            var discountCategory = await _discountCategoryRepo.GetDiscountCategorysAsync(id);
            return discountCategory == null ? NotFound() : Ok(discountCategory);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddNewdiscountCategory([FromBody] DiscountCategoryDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }
            var newdiscountCategoryId = await _discountCategoryRepo.AddDiscountCategoryAsync(dto);
            var discountCategory = await _discountCategoryRepo.GetDiscountCategorysAsync(newdiscountCategoryId);
            return discountCategory == null ? NotFound() : Ok(discountCategory);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdatediscountCategory(int id, [FromBody] DiscountCategoryDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var existingdiscountCategory = await _discountCategoryRepo.GetDiscountCategorysAsync(id);
            if (existingdiscountCategory == null)
            {
                return NotFound($"Không tìm thấy danh mục có ID = {id}");
            }

            existingdiscountCategory.Name = dto.Name;

            await _discountCategoryRepo.UpdateDiscountCategoryAsync(id, existingdiscountCategory);
            return Ok(existingdiscountCategory);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeletediscountCategory([FromRoute] int id)
        {
            var message = await _discountCategoryRepo.DeleteDiscountCategoryAsync(id);
            return Ok(new { message });
        }
    }
}