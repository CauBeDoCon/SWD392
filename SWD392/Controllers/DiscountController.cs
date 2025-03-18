using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWD392.DTOs;
using SWD392.Repositories;

namespace SWD392.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountRepository _discountRepo;

        public DiscountController(IDiscountRepository repo)
        {
            _discountRepo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDiscount([FromQuery] int? pageNumber, [FromQuery] int? pageSize)
        {
            int currentPage = pageNumber ?? 1;
            int currentSize = pageSize ?? 10;

            var result = await _discountRepo.GetAllDiscountAsync(currentPage, currentSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDiscountById(int id)
        {
            var Discount = await _discountRepo.GetDiscountAsync(id);
            return Discount == null ? NotFound() : Ok(Discount);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddNewDiscount([FromBody] DiscountRequestDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }
             if (dto.StartDate == dto.EndDate )
            {
                return BadRequest("❌ Ngày bắt đầu và ngày kết thúc không được giống nhau.");
            }else if(dto.StartDate > dto.EndDate){
                return BadRequest("❌Ngày kết thúc phai lon hon Ngày bắt đầu");
            }

            var newDiscountId = await _discountRepo.AddDiscountAsync(dto);
            var Discount = await _discountRepo.GetDiscountAsync(newDiscountId);
            return Discount == null ? NotFound() : Ok(Discount);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateDiscount(int id, [FromBody] DiscountDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var existingDiscount = await _discountRepo.GetDiscountAsync(id);
            if (existingDiscount == null)
            {
                return NotFound($"Không tìm thấy thể loại có ID = {id}");
            }

            existingDiscount.Code = dto.Code;
            existingDiscount.Description = dto.Description;
            existingDiscount.StartDate = dto.StartDate;   
            existingDiscount.EndDate = dto.EndDate;
            existingDiscount.max_usage = dto.max_usage;
            existingDiscount.Percentage= dto.Percentage;
            existingDiscount.DiscountCategoryId = dto.DiscountCategoryId;

            await _discountRepo.UpdateDiscountAsync(id, existingDiscount);
            return Ok(existingDiscount);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteDiscount([FromRoute] int id)
        {
            var message = await _discountRepo.DeleteDiscountAsync(id);
            return Ok(new { message });
        }
    }
}