using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWD392.DB;
using SWD392.DTOs.Pagination;
using SWD392.Models;
using SWD392.Repositories;

namespace SWD392.Repositories
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public OrderDetailRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<int> AddOrderDetailAsync(OrderDetailModel model)
        {
            var newOrderDetail = _mapper.Map<OrderDetail>(model);
            _context.OrderDetails!.Add(newOrderDetail);
            await _context.SaveChangesAsync();
            return newOrderDetail.Id;
        }

        public async Task<string> DeleteOrderDetailAsync(int id)
        {
            var deleteOrderDetail = await _context.OrderDetails!.FindAsync(id);

            if (deleteOrderDetail == null)
            {
                throw new KeyNotFoundException($"Chi tiết đơn hàng với ID {id} không tìm thấy.");
            }

            _context.OrderDetails.Remove(deleteOrderDetail);
            await _context.SaveChangesAsync();

            return $"Chi tiết đơn hàng với ID {id} đã xoá thành công.";
        }

        public async Task<PagedResult<OrderDetailModel>> GetAllOrderDetailsAsync(int pageNumber, int pageSize)
        {
            int totalCount = await _context.OrderDetails!.CountAsync();

            var orderDetails = await _context.OrderDetails!
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var mappedData = _mapper.Map<List<OrderDetailModel>>(orderDetails);

            return new PagedResult<OrderDetailModel>
            {
                Items = mappedData,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<OrderDetailModel> GetOrderDetailsAsync(int id)
        {
            var orderDetail = await _context.OrderDetails.FindAsync(id);

            if (orderDetail == null)
            {
                throw new KeyNotFoundException($"Chi tiết đơn hàng với ID {id} không tìm thấy.");
            }

            return _mapper.Map<OrderDetailModel>(orderDetail);
        }

        public async Task UpdateOrderDetailAsync(int id, OrderDetailModel model)
        {
            if (id != model.Id)
            {
                throw new ArgumentException("ID không khớp giữa request và model.");
            }

            var existingEntity = await _context.OrderDetails!.FindAsync(id);
            if (existingEntity == null)
            {
                throw new KeyNotFoundException($"Chi tiết đơn hàng với ID {id} không tìm thấy.");
            }

            _context.Entry(existingEntity).State = EntityState.Detached;

            var updateOrderDetail = _mapper.Map<OrderDetail>(model);

            _context.OrderDetails.Attach(updateOrderDetail);
            _context.Entry(updateOrderDetail).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }
    }
}

