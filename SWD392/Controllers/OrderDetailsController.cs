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
    public class OrderDetailsController : ControllerBase
    {
        private readonly IOrderDetailRepository _orderDetailRepo;

        public OrderDetailsController(IOrderDetailRepository repo)
        {
            _orderDetailRepo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrderDetails([FromQuery] int? pageNumber, [FromQuery] int? pageSize)
        {
            int currentPage = pageNumber ?? 1;
            int currentSize = pageSize ?? 10;

            var result = await _orderDetailRepo.GetAllOrderDetailsAsync(currentPage, currentSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderDetailById(int id)
        {
            try
            {
                var orderDetail = await _orderDetailRepo.GetOrderDetailsAsync(id);
                return Ok(orderDetail);
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
        public async Task<IActionResult> AddNewOrderDetail([FromBody] UpdateOrderDetailDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var model = new OrderDetailModel
            {
                OrderId = dto.OrderId,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                Subtotal = dto.Subtotal
            };

            var newOrderDetailId = await _orderDetailRepo.AddOrderDetailAsync(model);
            var orderDetail = await _orderDetailRepo.GetOrderDetailsAsync(newOrderDetailId);
            return orderDetail == null ? NotFound() : Ok(orderDetail);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateOrderDetail(int id, [FromBody] UpdateOrderDetailDto dto)
        {
            if (dto == null)
            {
                return BadRequest(new { message = "Dữ liệu không hợp lệ." });
            }

            try
            {
                var existingOrderDetail = await _orderDetailRepo.GetOrderDetailsAsync(id);
                existingOrderDetail.OrderId = dto.OrderId;
                existingOrderDetail.ProductId = dto.ProductId;
                existingOrderDetail.Quantity = dto.Quantity;
                existingOrderDetail.Subtotal = dto.Subtotal;

                await _orderDetailRepo.UpdateOrderDetailAsync(id, existingOrderDetail);
                return Ok(existingOrderDetail);
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
        public async Task<IActionResult> DeleteOrderDetail([FromRoute] int id)
        {
            try
            {
                var message = await _orderDetailRepo.DeleteOrderDetailAsync(id);
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
