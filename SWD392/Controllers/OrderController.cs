using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWD392.DTOs;

using SWD392.Repositories;
namespace SWD392.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrder([FromQuery] int? pageNumber, [FromQuery] int? pageSize)
        {
            int currentPage = pageNumber ?? 1;
            int currentSize = pageSize ?? 10;

            var result = await _orderRepository.GetAllOrdersPage(currentPage, currentSize);
            return Ok(result);
        }
        [HttpPost]
        public async Task<ActionResult<OrderResponse>> CreateOrder(OrderDTO orderDto)
        {
            var result = await _orderRepository.CreateOrderAsync(orderDto,User);
            if (result == null)
            {
                return BadRequest("Order creation failed.");
            }

            return CreatedAtAction(nameof(GetOrder), new { id = result.orderID }, result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderResponse>> GetOrder(int id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return order;
        }

        [HttpGet ("GetAllOrderByCustomerId/{id}")]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetAllOrderByCustomerId(String id)
        {
            return Ok(await _orderRepository.GetAllOrderByCustomerId(id));
        }
        [HttpGet("GetOrders")]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrders()
        {
            return Ok(await _orderRepository.GetOrdersAsync());
        }
        
        [HttpGet("GetAllOrdersByCancelStatus")]
        public async Task<ActionResult<IEnumerable<OrderCheckDto>>> GetAllOrdersByCancelStatus()
        {
            return Ok(await _orderRepository.GetAllOrdersByCancelStatus());
        }
         [HttpGet("GetAllOrdersByPendingStatus")]
        public async Task<ActionResult<IEnumerable<OrderCheckDto>>> GetAllOrdersByPendingStatus()
        {
            return Ok(await _orderRepository.GetAllOrdersByPendingStatus());
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, OrderResponse orderDto)
        {
            var result = await _orderRepository.UpdateOrderAsync(id, orderDto);
            if (!result) return NotFound();
            return NoContent();
        }
        [HttpPut("CancelStatusOrder/{id}")]
        public async Task<IActionResult> CancelStatusOrder(int id)
        {
            var result = await _orderRepository.CancelStatusOrder(id);
            if (result == 0) return NotFound();
            return NoContent();
        }

        [HttpPut("ConfirmCancelStatusOrder/{id}")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> ConfirmCancelStatusOrder(int id)
        {
            var result = await _orderRepository.ConfirmCancelStatusOrder(id);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPut("ConfirmOrderStatusOrder/{id}")]
        public async Task<IActionResult> ConfirmOrderStatusOrder(int id)
        {
            var result = await _orderRepository.ConfirmOrderStatusOrder(id);
            if (result == 0) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var result = await _orderRepository.DeleteOrderAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
