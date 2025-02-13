using Microsoft.AspNetCore.Authorization;
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
        public async Task<IActionResult> GetAllImages()
        {
            return Ok(await _imageRepo.GetAllImagesAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetImageById(int id)
        {
            var image = await _imageRepo.GetImagesAsync(id);
            return image == null ? NotFound() : Ok(image);
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
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var existingImage = await _imageRepo.GetImagesAsync(id);
            if (existingImage == null)
            {
                return NotFound($"Không tìm thấy hình ảnh có ID = {id}");
            }

            existingImage.ImageUrl = dto.ImageUrl;
            existingImage.ProductId = dto.ProductId;

            await _imageRepo.UpdateImageAsync(id, existingImage);
            return Ok(existingImage);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteImage([FromRoute] int id)
        {
            await _imageRepo.DeleteImageAsync(id);
            return Ok();
        }
    }
}
