using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using SWD392.DB;
using SWD392.DTOs;
using SWD392.DTOs.Pagination;

namespace SWD392.Repositories
{
    public interface IOrderRepository
    {
    Task<PagedResult<OrderCheckDto>> GetAllOrdersPage(int pageNumber, int pageSize);
    Task<List<OrderCheckDto>>GetAllOrders();
    Task<OrderResponse> CreateOrderAsync(OrderDTO orderDto, ClaimsPrincipal user);
    Task<OrderResponse?> GetOrderByIdAsync(int id);
    Task<IEnumerable<OrderResponse>> GetOrdersAsync();
    Task<IEnumerable<OrderResponse>> GetAllOrderByCustomerId(string id);

    Task<bool> UpdateOrderAsync(int id, OrderResponse orderDto);
    Task<bool> DeleteOrderAsync(int id);
    Task<int> CancelStatusOrder(int id);
    Task<OrderProcessResult> ConfirmCancelStatusOrder(int id);
    Task<int> ConfirmOrderStatusOrder(int id);
    Task<List<OrderCheckDto>> GetAllOrdersByCancelStatus();
    Task<List<OrderCheckDto>> GetAllOrdersByPendingStatus();
    Task<decimal> getProfitByMonth(int month, int year);
    Task<List<Order>> getAllHistoryOrderByMonthAndYear( int month, int year);
    Task<List<ProfitResponseDTO>> getProfit() ;
        
    }
}