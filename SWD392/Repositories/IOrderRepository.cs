using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using SWD392.DTOs;

namespace SWD392.Repositories
{
    public interface IOrderRepository
    {
    Task<OrderResponse> CreateOrderAsync(OrderDTO orderDto, ClaimsPrincipal user);
    Task<OrderResponse?> GetOrderByIdAsync(int id);
    Task<IEnumerable<OrderResponse>> GetOrdersAsync();
    Task<IEnumerable<OrderResponse>> GetAllOrderByCustomerId(string id);

    Task<bool> UpdateOrderAsync(int id, OrderResponse orderDto);
    Task<bool> DeleteOrderAsync(int id);
    }
}