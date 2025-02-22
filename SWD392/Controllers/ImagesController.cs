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
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository _imageRepo;

        public ImagesController(IImageRepository repo)
        {
            _imageRepo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllImages([FromQuery] int? pageNumber, [FromQuery] int? pageSize)
        {
            int currentPage = pageNumber ?? 1;
            int currentSize = pageSize ?? 10;

            var result = await _imageRepo.GetAllImagesAsync(currentPage, currentSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetImageById(int id)
        {
            try
            {
                var image = await _imageRepo.GetImagesAsync(id);
                return Ok(image);
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
        public async Task<IActionResult> AddNewImage([FromBody] UpdateImageDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var model = new ImageModel
            {
                ImageUrl = dto.ImageUrl,
                ProductId = dto.ProductId
            };

            var newImageId = await _imageRepo.AddImageAsync(model);
            var image = await _imageRepo.GetImagesAsync(newImageId);
            return image == null ? NotFound() : Ok(image);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateImage(int id, [FromBody] UpdateImageDto dto)
        {
            if (dto == null)
            {
                return BadRequest(new { message = "Dữ liệu không hợp lệ." });
            }

            try
            {
                var existingImage = await _imageRepo.GetImagesAsync(id);
                existingImage.ImageUrl = dto.ImageUrl;
                existingImage.ProductId = dto.ProductId;

                await _imageRepo.UpdateImageAsync(id, existingImage);
                return Ok(existingImage);
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
        public async Task<IActionResult> DeleteImage([FromRoute] int id)
        {
            try
            {
                var message = await _imageRepo.DeleteImageAsync(id);
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
