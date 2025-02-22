using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWD392.DTOs;
using SWD392.Models;
using SWD392.Repositories;
using System.Threading.Tasks;

namespace SWD392.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManufacturedCountriesController : ControllerBase
    {
        private readonly IManufacturedCountryRepository _manufacturedCountryRepo;

        public ManufacturedCountriesController(IManufacturedCountryRepository repo)
        {
            _manufacturedCountryRepo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllManufacturedCountries([FromQuery] int? pageNumber, [FromQuery] int? pageSize)
        {
            int currentPage = pageNumber ?? 1;
            int currentSize = pageSize ?? 10;

            var result = await _manufacturedCountryRepo.GetAllManufacturedCountriesAsync(currentPage, currentSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetManufacturedCountryById(int id)
        {
            try
            {
                var manufacturedCountry = await _manufacturedCountryRepo.GetManufacturedCountriesAsync(id);
                return Ok(manufacturedCountry);
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
        public async Task<IActionResult> AddNewManufacturedCountry([FromBody] UpdateManufacturedCountryDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var model = new ManufacturedCountryModel
            {
                Name = dto.Name
            };

            var newManufacturedCountryId = await _manufacturedCountryRepo.AddManufacturedCountryAsync(model);
            var manufacturedCountry = await _manufacturedCountryRepo.GetManufacturedCountriesAsync(newManufacturedCountryId);
            return manufacturedCountry == null ? NotFound() : Ok(manufacturedCountry);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateManufacturedCountry(int id, [FromBody] UpdateManufacturedCountryDto dto)
        {
            if (dto == null)
            {
                return BadRequest(new { message = "Dữ liệu không hợp lệ." });
            }

            try
            {
                var existingManufacturedCountry = await _manufacturedCountryRepo.GetManufacturedCountriesAsync(id);
                existingManufacturedCountry.Name = dto.Name;

                await _manufacturedCountryRepo.UpdateManufacturedCountryAsync(id, existingManufacturedCountry);
                return Ok(existingManufacturedCountry);
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
        public async Task<IActionResult> DeleteManufacturedCountry([FromRoute] int id)
        {
            try
            {
                var message = await _manufacturedCountryRepo.DeleteManufacturedCountryAsync(id);
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
