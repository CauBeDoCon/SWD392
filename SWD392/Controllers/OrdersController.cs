using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWD392.DTOs;
using SWD392.Models;
using SWD392.Repositories;
using System.Security.Claims;

namespace SWD392.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepo;

        public OrdersController(IOrderRepository repo)
        {
            _orderRepo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders([FromQuery] int? pageNumber, [FromQuery] int? pageSize)
        {
            int currentPage = pageNumber ?? 1;
            int currentSize = pageSize ?? 10;

            var result = await _orderRepo.GetAllOrdersAsync(currentPage, currentSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            try
            {
                var order = await _orderRepo.GetOrdersAsync(id);
                return Ok(order);
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
        public async Task<IActionResult> AddNewOrder([FromBody] UpdateOrderDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var model = new OrderModel
            {
                UserId = dto.UserId,
                OrderDate = dto.OrderDate,
                TotalAmount = dto.TotalAmount,
                Status = dto.Status,
                CartId = dto.CartId
            };

            var newOrderId = await _orderRepo.AddOrderAsync(model);
            var order = await _orderRepo.GetOrdersAsync(newOrderId);
            return order == null ? NotFound() : Ok(order);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] UpdateOrderDto dto)
        {
            if (dto == null)
            {
                return BadRequest(new { message = "Dữ liệu không hợp lệ." });
            }

            try
            {
                var existingOrder = await _orderRepo.GetOrdersAsync(id);
                existingOrder.UserId = dto.UserId;
                existingOrder.OrderDate = dto.OrderDate;
                existingOrder.TotalAmount = dto.TotalAmount;
                existingOrder.Status = dto.Status;
                existingOrder.CartId = dto.CartId;

                await _orderRepo.UpdateOrderAsync(id, existingOrder);
                return Ok(existingOrder);
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

        [HttpPost("place-order/{cartId}")]
        [Authorize]
        public async Task<IActionResult> PlaceOrder(int cartId)
        {
            // Lấy userId từ token
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User không hợp lệ." });
            }

            var result = await _orderRepo.PlaceOrderAsync(cartId, userId);
            if (!result)
            {
                return BadRequest(new { message = "Đặt hàng thất bại: không đủ số lượng tồn kho hoặc lỗi khác." });
            }

            return Ok(new { message = "Đặt hàng thành công!" });
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteOrder([FromRoute] int id)
        {
            try
            {
                var message = await _orderRepo.DeleteOrderAsync(id);
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
