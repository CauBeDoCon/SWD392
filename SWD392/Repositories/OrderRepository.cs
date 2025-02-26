using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWD392.DB;
using SWD392.DTOs.Pagination;
using SWD392.Models;
using SWD392.Repositories;

namespace SWD392.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public OrderRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<int> AddOrderAsync(OrderModel model)
        {
            var newOrder = _mapper.Map<Order>(model);
            _context.Orders!.Add(newOrder);
            await _context.SaveChangesAsync();
            return newOrder.OrderId;
        }

        public async Task<string> DeleteOrderAsync(int id)
        {
            var deleteOrder = await _context.Orders!.FindAsync(id);

            if (deleteOrder == null)
            {
                throw new KeyNotFoundException($"Đơn hàng với ID {id} không tìm thấy.");
            }

            _context.Orders.Remove(deleteOrder);
            await _context.SaveChangesAsync();

            return $"Đơn hàng với ID {id} đã xoá thành công.";
        }

        public async Task<PagedResult<OrderModel>> GetAllOrdersAsync(int pageNumber, int pageSize)
        {
            int totalCount = await _context.Orders!.CountAsync();

            var orders = await _context.Orders!
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var mappedData = _mapper.Map<List<OrderModel>>(orders);

            return new PagedResult<OrderModel>
            {
                Items = mappedData,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<OrderModel> GetOrdersAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                throw new KeyNotFoundException($"Đơn hàng với ID {id} không tìm thấy.");
            }

            return _mapper.Map<OrderModel>(order);
        }

        public async Task UpdateOrderAsync(int id, OrderModel model)
        {
            if (id != model.OrderId)
            {
                throw new ArgumentException("ID không khớp giữa request và model.");
            }

            var existingEntity = await _context.Orders!.FindAsync(id);
            if (existingEntity == null)
            {
                throw new KeyNotFoundException($"Đơn hàng với ID {id} không tìm thấy.");
            }

            _context.Entry(existingEntity).State = EntityState.Detached;

            var updateOrder = _mapper.Map<Order>(model);

            _context.Orders.Attach(updateOrder);
            _context.Entry(updateOrder).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }
    }
}
