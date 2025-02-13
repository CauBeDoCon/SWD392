using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWD392.DTOs;
using SWD392.Models;
using SWD392.Repositories;

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
        public async Task<IActionResult> GetAllManufacturedCountries()
        {
            return Ok(await _manufacturedCountryRepo.GetAllManufacturedCountriesAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetManufacturedCountryById(int id)
        {
            var manufacturedCountry = await _manufacturedCountryRepo.GetManufacturedCountriesAsync(id);
            return manufacturedCountry == null ? NotFound() : Ok(manufacturedCountry);
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
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var existingCountry = await _manufacturedCountryRepo.GetManufacturedCountriesAsync(id);
            if (existingCountry == null)
            {
                return NotFound($"Không tìm thấy nước sản xuất có ID = {id}");
            }

            existingCountry.Name = dto.Name;

            await _manufacturedCountryRepo.UpdateManufacturedCountryAsync(id, existingCountry);
            return Ok(existingCountry);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteManufacturedCountry([FromRoute] int id)
        {
            await _manufacturedCountryRepo.DeleteManufacturedCountryAsync(id);
            return Ok();
        }
    }
}
