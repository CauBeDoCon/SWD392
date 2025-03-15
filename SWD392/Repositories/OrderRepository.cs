using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SWD392.DB;
using SWD392.DTOs;
using SWD392.DTOs.Pagination;
using SWD392.enums;

namespace SWD392.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IWalletRepository _walletRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderRepository> _logger;

        public OrderRepository(  ILogger<OrderRepository> logger,IMapper mapper,ApplicationDbContext context, IWalletRepository walletRepository, ICartRepository cartRepository)
        {
            _context = context;
            _walletRepository = walletRepository;
            _cartRepository = cartRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PagedResult<OrderCheckDto>> GetAllOrdersPage(int pageNumber, int pageSize){
            int totalCount = await _context.manufacturedCountries!.CountAsync();

            var OrderCheckDto = await _context.Orders!
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var mappedData = _mapper.Map<List<OrderCheckDto>>(OrderCheckDto);

            return new PagedResult<OrderCheckDto>
            {
                Items = mappedData,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
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
             var newOrder = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                TotalAmount = totalAmount,
                Status = OrderStatus.pending,
                IsRefunded = false,
                DiscountId = orderDto.DiscountId,
                CartId = cart.Id,
            };
            if (discount != null && discount.max_usage > 0)
            {
                discount.max_usage -= 1;
                _context.Entry(discount).Property(p => p.max_usage).IsModified = true;
                totalAmount -= totalAmount * discount.Percentage / 100;
            }
            else {
                return new OrderResponse
                {
                    orderID = 0,
                    applicationUserID = newOrder.UserId,
                    OrderDate = DateTime.Now,
                    TotalAmount = 0,
                    IsRefunded = newOrder.IsRefunded,
                    Status = "failed",
                    DiscountId = 0,
                    CartId = newOrder.CartId,
                    Message = "Ma giam gia da het !"
                };
            }
            var wallet = await _walletRepository.GetWalletBalanceAsync(userId);
            if (wallet.AmountofMoney < newOrder.TotalAmount)
            {
                return new OrderResponse
                {
                    orderID = 0,
                    applicationUserID = newOrder.UserId,
                    OrderDate = DateTime.Now,
                    TotalAmount = 0,
                    IsRefunded = newOrder.IsRefunded,
                    Status = "failed",
                    DiscountId = 0,
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
                IsRefunded = newOrder.IsRefunded,
                TotalAmount = newOrder.TotalAmount,
                Status = newOrder.Status.ToString(),
                DiscountId = newOrder.DiscountId.Value,
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
                DiscountId = order.DiscountId.Value,
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
                    DiscountId = o.DiscountId.Value,
                    CartId = o.CartId
                })
                .ToListAsync();
        }

        public async Task<bool> UpdateOrderAsync(int id, OrderResponse orderDto)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return false;
            order.IsRefunded = orderDto.IsRefunded;
            order.UserId =Convert.ToString(orderDto.applicationUserID) ;
            order.OrderDate = orderDto.OrderDate;
            order.TotalAmount = orderDto.TotalAmount;
            order.Status = Enum.Parse<OrderStatus>(orderDto.Status);
            order.DiscountId = orderDto.DiscountId;
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
                    DiscountId = o.DiscountId.Value,
                    CartId = o.CartId
                })
                .ToListAsync();
        }
        public async Task<int> CancelStatusOrder(int id){
            var order = await _context.Orders.Where(o =>o.OrderId==id).FirstOrDefaultAsync();
            if(order!=null){
                order.Status = OrderStatus.canceled;
                order.CancelledDate = DateTime.Now;
                await _context.SaveChangesAsync();
                return order.OrderId;
            }
            return 0; // or throw an exception
        }

    
        public async Task<List<OrderCheckDto>> GetAllOrders()
        {
            var Orders = await _context.Orders.ToListAsync();
            var mapper = _mapper.Map<List<OrderCheckDto>>(Orders);
            return mapper;
        }
        
        public async Task<OrderProcessResult> ConfirmCancelStatusOrder(int id){
            var order = await _context.Orders.Include(o => o.OrderDetails).FirstOrDefaultAsync(o => o.OrderId == id);
            if (order == null)
            {
                return new OrderProcessResult
                {
                    Success = false,
                    Message = "Không tìm thấy đơn hàng!",
                    OrderId = null
                };
            }

            // Bước 1: Nếu đơn hàng đang ở trạng thái canceled -> xác nhận hủy
            if (order.Status == OrderStatus.canceled)
            {
                order.Status = OrderStatus.Confirmcanceled;
                await _context.SaveChangesAsync();
            }

            // Bước 2: Kiểm tra và xử lý hoàn tiền nếu đã Confirmcanceled
            if (order.Status == OrderStatus.Confirmcanceled && order.CancelledDate != null && !order.IsRefunded)
            {
                var hoursSinceCanceled = DateTime.Now.Subtract(order.CancelledDate.Value).TotalHours;
                var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.User.Id == order.UserId);
                if (wallet == null)
                {
                    return new OrderProcessResult
                    {
                        Success = false,
                        Message = "Không tìm thấy ví người dùng!",
                        OrderId = order.OrderId
                    };
                }

                decimal refundAmount = 0;
                if (hoursSinceCanceled < 24)
                {
                    refundAmount = order.TotalAmount;
                }
                else if (hoursSinceCanceled >= 24 && hoursSinceCanceled <= 48)
                {
                    refundAmount = order.TotalAmount * 0.8m;
                }
                else
                {
                    return new OrderProcessResult
                    {
                        Success = false,
                        Message = "Không thể hoàn tiền do đã quá 48h.",
                        OrderId = order.OrderId
                    };
                }

                // Cập nhật ví
                wallet.AmountOfMoney += (int)refundAmount;
                // ✅ Cộng lại quantity cho từng sản phẩm
                foreach (var orderDetail in order.OrderDetails)
                {
                    var product = await _context.products.FirstOrDefaultAsync(p => p.Id == orderDetail.ProductId);
                    if (product != null)
                    {
                        product.Quantity += orderDetail.Quantity;
                        _logger.LogInformation($"Cộng lại {orderDetail.Quantity} sản phẩm {product.Name} về kho.");
                    }
                }

                var discount = await _context.discounts.FirstOrDefaultAsync(d => d.Id == order.DiscountId.Value);
                if (discount != null)
                {
                    discount.max_usage += 1;
                    _logger.LogInformation($"Tăng số lần sử dụng của mã giảm giá {discount.Code} lên {discount.max_usage}.");
                }
                // Đánh dấu đã hoàn tiền
                order.IsRefunded = true;
                // Cập nhật database
                await _context.SaveChangesAsync();

                return new OrderProcessResult
                {
                    Success = true,
                    Message = $"Đã hoàn tiền {refundAmount} VNĐ cho đơn hàng {order.OrderId}.",
                    OrderId = order.OrderId
                };
            }

            return new OrderProcessResult
            {
                Success = false,
                Message = "Đơn hàng không hợp lệ để hoàn tiền hoặc đã hoàn trước đó.",
                OrderId = order.OrderId
            };
        }
        public async Task<int> ConfirmOrderStatusOrder(int id){
            var order = await _context.Orders.Where(o =>o.OrderId==id&&o.Status==OrderStatus.pending).FirstOrDefaultAsync();
            if(order!=null){
                order.Status = OrderStatus.successful;
                await _context.SaveChangesAsync();
                return order.OrderId;
            }
            return 0; // or throw an exception
        }
        public async Task<List<OrderCheckDto>> GetAllOrdersByCancelStatus()
        {
            var Orders = await _context.Orders.Where(o=>o.Status==OrderStatus.canceled).ToListAsync();
            var mapper = _mapper.Map<List<OrderCheckDto>>(Orders);
            return mapper;
        }
        public async Task<List<OrderCheckDto>> GetAllOrdersByPendingStatus()
        {
            var Orders = await _context.Orders.Where(o=>o.Status==OrderStatus.pending).ToListAsync();
            var mapper = _mapper.Map<List<OrderCheckDto>>(Orders);
            return mapper;
        }
        public async Task<decimal> getProfitByMonth(int month, int year)
        {
            var orders = await _context.Orders.Where(o => o.OrderDate.Month == month && o.OrderDate.Year == year).ToListAsync();
            decimal profit = 0;
            foreach (var order in orders)
            {
                profit += order.TotalAmount;
            }
            return profit;
        }
        public async Task<List<Order>> getAllHistoryOrderByMonthAndYear( int month, int year)
        {
            var orders = await _context.Orders.Where(o=> o.OrderDate.Month == month && o.OrderDate.Year == year).ToListAsync();
            return orders;
        }
        public async Task<List<ProfitResponseDTO>> getProfit() {
        
            List<ProfitResponseDTO> list = new List<ProfitResponseDTO>();
            decimal revenuePortal=0;
            List<Order> systemProfits = new List<Order>(); // Khai báo ngoài try/catch
            var monthnow = DateTime.Now;
            var monthValue = monthnow.Month;
            var year = monthnow.Year;
            var month1 = monthValue - 7;// check co phai 7 month trong nam ko
            var monthLack = 0;
            List<Order> orders = new List<Order>();
            if (month1 < 0) {
                monthLack = month1 * (-1);
                // nam truoc
                for (int i = 12 - monthLack+1; i <= 12; i++) {
                    int month = i;
                    List<OrderCheckDto> listOrderCheckDtoMapByDTOS = new List<OrderCheckDto>();
                    try {
                        revenuePortal = await getProfitByMonth(month, year - 1);
                        systemProfits = await getAllHistoryOrderByMonthAndYear(month, year - 1);
                    } catch (Exception e) { 
                        revenuePortal = 0;
                        systemProfits = new List<Order>();
                    }
                    List<OrderCheckDto> orderDtos = new List<OrderCheckDto>();
                    foreach (var systemProfit in systemProfits) 
                    {
                        OrderCheckDto dto = new OrderCheckDto
                            {
                                orderID = systemProfit.OrderId,
                                applicationUserID = systemProfit.UserId,
                                OrderDate = systemProfit.OrderDate,
                                CanceledDate = systemProfit.CancelledDate,
                                TotalAmount = systemProfit.TotalAmount,
                                IsRefunded = systemProfit.IsRefunded,
                                Status = systemProfit.Status.ToString(),
                                DiscountId = systemProfit.DiscountId ?? 0,
                                CartId = systemProfit.CartId
                            };
                         orderDtos.Add(dto);
                    }
                        
                    
                    ProfitResponseDTO responseDTO = new ProfitResponseDTO
                        {
                            month = month,
                            revenuePortal = revenuePortal,
                            orderCheckDtos = orderDtos
                        };
                    list.Add(responseDTO);
                }
            }
        
            // nam hien tai
            for (int i = monthValue - 7 + monthLack+1; i <= monthValue; i++) {  
                int month = i;
                List<OrderCheckDto> listSystemProfitMapByDTOS = new List<OrderCheckDto>();
                try {
                    revenuePortal = await getProfitByMonth(month, year);
                    systemProfits = await getAllHistoryOrderByMonthAndYear(month, year);
                } catch (Exception e) {
                    revenuePortal = 0;
                    systemProfits = new List<Order>();
                }
                List<OrderCheckDto> orderDtos = new List<OrderCheckDto>();
                foreach (var systemProfit in systemProfits) 
                    {
                        OrderCheckDto dto = new OrderCheckDto
                            {
                                orderID = systemProfit.OrderId,
                                applicationUserID = systemProfit.UserId,
                                OrderDate = systemProfit.OrderDate,
                                CanceledDate = systemProfit.CancelledDate,
                                TotalAmount = systemProfit.TotalAmount,
                                IsRefunded = systemProfit.IsRefunded,
                                Status = systemProfit.Status.ToString(),
                                DiscountId = systemProfit.DiscountId ?? 0,
                                CartId = systemProfit.CartId
                            };
                         orderDtos.Add(dto);
                    }
                ProfitResponseDTO responseDTO = new ProfitResponseDTO
                        {
                            month = month,
                            revenuePortal = revenuePortal,
                            orderCheckDtos = orderDtos
                        };
                    list.Add(responseDTO);
            }

            return list;
        }
    }
}