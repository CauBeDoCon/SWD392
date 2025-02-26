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

        public async Task<bool> PlaceOrderAsync(int cartId, string userId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Lấy giỏ hàng kèm thông tin CartProducts và Product
                var cart = await _context.carts
                    .Include(c => c.CartProducts)
                        .ThenInclude(cp => cp.Product)
                    .FirstOrDefaultAsync(c => c.Id == cartId);

                if (cart == null || !cart.CartProducts.Any())
                {
                    return false; // Giỏ hàng trống hoặc không tồn tại
                }

                // Tính tổng tiền đơn hàng
                double totalAmount = cart.CartProducts.Sum(cp => cp.Quantity * cp.Product.Price);

                // Kiểm tra tồn kho của từng sản phẩm
                foreach (var cp in cart.CartProducts)
                {
                    if (cp.Product.StockRemaining < cp.Quantity)
                    {
                        throw new Exception($"Sản phẩm {cp.Product.Name} không đủ số lượng tồn kho.");
                    }
                }

                // Tạo Order với trạng thái ban đầu "Pending"
                var order = new Order
                {
                    UserId = userId,
                    OrderDate = DateTime.UtcNow,
                    TotalAmount = (decimal)totalAmount,
                    Status = "Pending",
                    CartId = cartId
                };
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                // Xử lý từng sản phẩm trong giỏ hàng
                foreach (var cp in cart.CartProducts)
                {
                    // Trừ số lượng tồn kho
                    cp.Product.StockRemaining -= cp.Quantity;

                    // Tạo OrderDetail cho sản phẩm này
                    var orderDetail = new OrderDetail
                    {
                        OrderId = order.OrderId,
                        ProductId = cp.ProductId,
                        Quantity = cp.Quantity,
                        Subtotal = (decimal)(cp.Quantity * cp.Product.Price)
                    };
                    _context.OrderDetails.Add(orderDetail);

                    // Nếu muốn, cập nhật trạng thái của CartProduct
                    cp.Status = "Ordered";
                }

                // Xoá toàn bộ sản phẩm trong giỏ hàng sau khi đặt hàng thành công
                _context.cartProducts.RemoveRange(cart.CartProducts);

                // Cập nhật trạng thái đơn hàng thành "Success"
                order.Status = "Success";

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                // Có thể log lỗi nếu cần
                return false;
            }
        }
    }
}
