using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SWD392.DB;
using SWD392.DTOs;

namespace SWD392.Repositories
{
    public interface IOrderDetailRepository
{
    Task<IEnumerable<OrderDetailDTO>> GetOrderDetailsAsync();
    Task<List<OrderDetailDTO>> GetOrderDetailByIdAsync(int id);
    Task<OrderDetail> CreateOrderDetailAsync(OrderDetailDTO orderDetailDto);
    Task<List<OrderDetailDTO>> GetOrderDetailByOrderIdAsync(int id);
}

}