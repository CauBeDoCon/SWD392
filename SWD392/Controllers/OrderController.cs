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


    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrders()
    {
        return Ok(await _orderRepository.GetOrdersAsync());
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrder(int id, OrderResponse orderDto)
    {
        var result = await _orderRepository.UpdateOrderAsync(id, orderDto);
        if (!result) return NotFound();
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
