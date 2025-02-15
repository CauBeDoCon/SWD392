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
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepo;

        public CategoriesController(ICategoryRepository repo)
        {
            _categoryRepo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories([FromQuery] int? pageNumber, [FromQuery] int? pageSize)
        {
            int currentPage = pageNumber ?? 1;
            int currentSize = pageSize ?? 10;

            var result = await _categoryRepo.GetAllCategoriesAsync(currentPage, currentSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _categoryRepo.GetCategoriesAsync(id);
            return category == null ? NotFound() : Ok(category);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddNewCategory([FromBody] UpdateCategoryDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var model = new CategoryModel
            {
                Name = dto.Name,
                Image = dto.Image,
                SolutionId = dto.SolutionId
            };

            var newCategoryId = await _categoryRepo.AddCategoryAsync(model);
            var category = await _categoryRepo.GetCategoriesAsync(newCategoryId);
            return category == null ? NotFound() : Ok(category);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var existingCategory = await _categoryRepo.GetCategoriesAsync(id);
            if (existingCategory == null)
            {
                return NotFound($"Không tìm thấy thể loại có ID = {id}");
            }

            existingCategory.Name = dto.Name;
            existingCategory.Image = dto.Image;
            existingCategory.SolutionId = dto.SolutionId;   


            await _categoryRepo.UpdateCategoryAsync(id, existingCategory);
            return Ok(existingCategory);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteCategory([FromRoute] int id)
        {
            var message = await _categoryRepo.DeleteCategoryAsync(id);
            return Ok(new { message });
        }
    }
}
