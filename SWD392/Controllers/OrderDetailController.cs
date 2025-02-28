using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWD392.DB;
using SWD392.DTOs;
using SWD392.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWD392.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        private readonly IOrderDetailRepository _orderDetailRepository;

        public OrderDetailController(IOrderDetailRepository orderDetailRepository)
        {
            _orderDetailRepository = orderDetailRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDetailDTO>>> GetOrderDetailsAll()
        {
            var orderDetails = await _orderDetailRepository.GetOrderDetailsAsync();
            return Ok(orderDetails);
        }

        
        [HttpGet("GetOrderDetailsByOrderID/{id}")]
        public async Task<ActionResult<IEnumerable<OrderDetailDTO>>> GetOrderDetailsByOrderID(int id )
        {
            var orderDetails = await _orderDetailRepository.GetOrderDetailByOrderIdAsync(id);
            return Ok(orderDetails);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<List<OrderDetailDTO>>> GetOrderDetailByIdAsync(int id)
        {
            var orderDetails = await _orderDetailRepository.GetOrderDetailByIdAsync(id);
            if (orderDetails == null )
            {
                return NotFound();
            }
            return Ok(orderDetails);
        }

        [HttpPost]
        public async Task<ActionResult<OrderDetailDTO>> CreateOrderDetail(OrderDetailDTO orderDetailDto)
        {
            var orderDetail = await _orderDetailRepository.CreateOrderDetailAsync(orderDetailDto);
            return CreatedAtAction(nameof(GetOrderDetailsByOrderID), new { id = orderDetail.Id }, orderDetailDto);
        }
    }

}
