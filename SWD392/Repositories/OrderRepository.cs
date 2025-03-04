using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SWD392.DB;
using SWD392.DTOs;
using SWD392.enums;

namespace SWD392.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IWalletRepository _walletRepository;
        private readonly ICartRepository _cartRepository;

        public OrderRepository(ApplicationDbContext context, IWalletRepository walletRepository, ICartRepository cartRepository)
        {
            _context = context;
            _walletRepository = walletRepository;
            _cartRepository = cartRepository;
        }

        public async Task<OrderResponse> CreateOrderAsync(OrderDTO orderDto, ClaimsPrincipal user)
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var cart = await _context.carts
                .Include(c => c.CartProducts)
                .ThenInclude(cp => cp.Product)
                .FirstOrDefaultAsync(c => c.User.Id == userId);

            if (cart == null || cart.CartProducts.Count == 0)
            {
                return null;
            }

            var totalAmount = await _cartRepository.TotalPriceInCartProduct(Convert.ToInt32(cart.Id));
            var discount = await _context.discounts.FindAsync(orderDto.DiscountId);

            if (discount != null)
            {
                discount.max_usage -= 1;
                _context.Entry(discount).Property(p => p.max_usage).IsModified = true;
                totalAmount -= totalAmount * discount.Percentage / 100;
            }

            var newOrder = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                TotalAmount = totalAmount,
                Status = OrderStatus.pending,
                DiscountId = orderDto.DiscountId,
                CartId = cart.Id,
            };

            var wallet = await _walletRepository.GetWalletBalanceAsync(userId);
            if (wallet.AmountofMoney < newOrder.TotalAmount)
            {
                return new OrderResponse
                {
                    orderID = 0,
                    applicationUserID = newOrder.UserId,
                    OrderDate = DateTime.Now,
                    TotalAmount = 0,
                    Status = "failed",
                    Discount = null,
                    CartId = newOrder.CartId,
                    Message = "Số dư không đủ để thanh toán!"
                };
                throw new Exception("Số dư trong ví không đủ để thực hiện giao dịch.");
            }
            foreach (var cartProduct in cart.CartProducts)
            {
                var product = cartProduct.Product;
                product.Quantity -= cartProduct.Quantity;
                _context.Entry(product).Property(p => p.Quantity).IsModified = true;
            }       
            _context.Orders.Add(newOrder);
            await _context.SaveChangesAsync();

            var orderDetails = cart.CartProducts.Select(cartProduct => new OrderDetail
            {
                OrderId = newOrder.OrderId,
                ProductId = cartProduct.Product.Id,
                Subtotal = Convert.ToDecimal(cartProduct.Product.Price * cartProduct.Quantity),
                Quantity = cartProduct.Quantity
            }).ToList();

            _context.OrderDetails.AddRange(orderDetails);
            await _context.SaveChangesAsync();

            _context.cartProducts.RemoveRange(cart.CartProducts);
            await _context.SaveChangesAsync();

            var newBalance = new WalletDTO
            {
                WalletId = (int)cart.User.WalletId,
                AmountofMoney = (int)(wallet.AmountofMoney - newOrder.TotalAmount)
            };

            await _walletRepository.UpdateWalletBalanceAsync(newBalance);
            await _context.SaveChangesAsync();

            return new OrderResponse
            {
                orderID = newOrder.OrderId,
                applicationUserID = newOrder.UserId,
                OrderDate = newOrder.OrderDate,
                TotalAmount = newOrder.TotalAmount,
                Status = newOrder.Status.ToString(),
                Discount = newOrder.Discount,
                CartId = newOrder.CartId,
                Message = "Đặt hàng thành công!"
            };
        }

        public async Task<OrderResponse?> GetOrderByIdAsync(int id)
        {
            var order = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Discount)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null) return null;

            return new OrderResponse
            {
                orderID = order.OrderId,
                applicationUserID = order.UserId,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Status = order.Status.ToString(),
                Discount = order.Discount,
                CartId = order.CartId
            };
        }
        public async Task<IEnumerable<OrderResponse>> GetOrdersAsync()
        {
            return await _context.Orders
                .Select(o => new OrderResponse
                {
                    orderID = o.OrderId,

                    applicationUserID = o.UserId,
                    OrderDate = o.OrderDate,
                    TotalAmount = o.TotalAmount,
                    Status = o.Status.ToString(),
                    Discount = o.Discount,
                    CartId = o.CartId
                })
                .ToListAsync();
        }

        public async Task<bool> UpdateOrderAsync(int id, OrderResponse orderDto)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return false;

            order.UserId =Convert.ToString(orderDto.applicationUserID) ;
            order.OrderDate = orderDto.OrderDate;
            order.TotalAmount = orderDto.TotalAmount;
            order.Status = Enum.Parse<OrderStatus>(orderDto.Status);
            order.Discount = orderDto.Discount;
            order.CartId = orderDto.CartId;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteOrderAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return false;

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<OrderResponse>> GetAllOrderByCustomerId(string id)
        {
            return await _context.Orders.
                Where(o=>o.UserId== id).
                Select(o => new OrderResponse
                {
                    orderID = o.OrderId,
                    applicationUserID = o.UserId,
                    OrderDate = o.OrderDate,
                    TotalAmount = o.TotalAmount,
                    Status = o.Status.ToString(),
                    Discount = o.Discount,
                    CartId = o.CartId
                })
                .ToListAsync();
        }
    }

}