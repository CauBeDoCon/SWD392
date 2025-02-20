using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SWD392.DB;
using SWD392.DTOs;
using SWD392.Models;
using SWD392.Services;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SWD392.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] 
    public class WalletController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IVnPayService _vnPayService;
        public WalletController(ApplicationDbContext context, IVnPayService vnPayService)
        {
            _context = context;
            _vnPayService = vnPayService;
        }

    
        [HttpGet]
        public async Task<ActionResult<WalletDTO>> GetWalletBalance()
        {
        
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Invalid token or user not authenticated.");
            }

            var user = await _context.Users
                .Include(u => u.Wallet)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null || user.Wallet == null)
            {
                return NotFound("User or Wallet not found.");
            }

            return new WalletDTO
            {
                WalletId = user.Wallet.WalletId,
                AmountofMoney = (int)user.Wallet.AmountOfMoney
            };
        }

        [Authorize]
        [Authorize]
        [Authorize]
        [Authorize]
        [HttpPost("CreateVNPayPayment")]
        public IActionResult CreateVNPayPayment([FromBody] CreateVNPayRequestDTO request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { Message = "Bạn chưa đăng nhập hoặc token không hợp lệ." });
            }
            if (request.Amount <= 0)
            {
                return BadRequest(new { Message = "Số tiền không hợp lệ." });
            }

            var paymentRequest = new VnPaymentRequestModel
            {
                Amount = request.Amount,
                OrderId = new Random().Next(100000, 999999), 
                CreatedTime = DateTime.UtcNow
            };

            // ✅ Gọi service tạo URL thanh toán
            string paymentUrl = _vnPayService.CreatePaymentUrl(HttpContext, paymentRequest);

            return Ok(new { PaymentUrl = paymentUrl });
        }





    }
}
