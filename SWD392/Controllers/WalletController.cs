﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SWD392.DB;
using SWD392.DTOs;
using SWD392.Models;
using SWD392.Services;
using System.Security.Claims;
using System.Threading.Tasks;
using SWD392.Services;
using SWD392.Repositories;
namespace SWD392.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] 
    public class WalletController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IVnPayService _vnPayService;
        private readonly IWalletRepository _walletRepository;
        public WalletController(ApplicationDbContext context, IVnPayService vnPayService,IWalletRepository walletRepository)
        {
            _context = context;
            _vnPayService = vnPayService;
            _walletRepository = walletRepository;
        }

    
        [HttpGet]
        public async Task<ActionResult<WalletDTO>> GetWalletBalance()
        {
        
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("Invalid token or user not authenticated.");
        }

        var walletBalance = await _walletRepository.GetWalletBalanceAsync(userId);
        if (walletBalance == null)
        {
            return NotFound("User or Wallet not found.");
        }

        return Ok(walletBalance);
        }

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
                CreatedTime = DateTime.UtcNow
            };
            string paymentUrl = _vnPayService.CreatePaymentUrl(HttpContext, paymentRequest, userId);

            return Ok(new { PaymentUrl = paymentUrl });
        }

        [AllowAnonymous]
        [HttpGet("VNPayReturn")]
        public async Task<IActionResult> VNPayReturn()
        {
            try
            {
                var response = _vnPayService.PaymentExecute(Request.Query);
                if (!response.Success)
                {
                    return BadRequest(new { Message = "Giao dịch thất bại hoặc bị từ chối." });
                }
                var vnp_OrderInfo = response.OrderDescription;
                string vnp_UserId = vnp_OrderInfo.Replace("Thanh toán VNPay - UserId: ", "").Trim();

                Console.WriteLine($"🔹 VNPay trả về UserId: {vnp_UserId}");

                if (string.IsNullOrEmpty(vnp_UserId))
                {
                    return BadRequest(new { Message = "VNPay không gửi UserId hợp lệ." });
                }

                var vnp_AmountString = Request.Query["vnp_Amount"];
                if (string.IsNullOrEmpty(vnp_AmountString))
                {
                    return BadRequest(new { Message = "VNPay không gửi số tiền hợp lệ." });
                }

                decimal vnp_Amount = Convert.ToDecimal(vnp_AmountString) / 100;

                if (vnp_Amount <= 0)
                {
                    return BadRequest(new { Message = "Số tiền giao dịch không hợp lệ." });
                }

                var user = await _context.Users
                    .Include(u => u.Wallet)
                    .FirstOrDefaultAsync(u => u.Id == vnp_UserId);

                if (user == null)
                {
                    return NotFound(new { Message = "Không tìm thấy người dùng." });
                }

                if (user.Wallet == null)
                {
                    return NotFound(new { Message = "Người dùng chưa có ví." });
                }

                user.Wallet.AmountOfMoney += vnp_Amount;
                var depositTransaction = new Transaction
                {
                    WalletId = user.Wallet.WalletId,
                    Account = user.Email,
                    CreatedTransaction = DateTime.UtcNow,
                    BankName = "VNPay",
                    AccountName = user.FirstName,
                    AccountNumber = "VNPay",
                    Amount = vnp_Amount,
                    TransactionEnum = "Completed",
                    Type = "Deposit",
                    ReasonWithdrawReject = null
                };

                _context.Transactions.Add(depositTransaction);
                await _context.SaveChangesAsync();

                var responseCode = Request.Query["vnp_ResponseCode"];

                if (responseCode == "00")
                {
                    var vnpAmount = Request.Query["vnp_Amount"];
                    var vnpBankCode = Request.Query["vnp_BankCode"];
                    var vnpBankTranNo = Request.Query["vnp_BankTranNo"];
                    var vnpTransactionNo = Request.Query["vnp_TransactionNo"];
                    var vnpPayDate = Request.Query["vnp_PayDate"];

                    var redirectUrl = $"http://localhost:5173/deposite" +
                                      $"?Status=Success" +
                                      $"&vnp_ResponseCode={responseCode}" + 
                                      $"&vnp_Amount={vnpAmount}" +
                                      $"&vnp_BankCode={vnpBankCode}" +
                                      $"&vnp_BankTranNo={vnpBankTranNo}" +
                                      $"&vnp_TransactionNo={vnpTransactionNo}" +
                                      $"&vnp_PayDate={vnpPayDate}";

                    return Redirect(redirectUrl);
                }
                return BadRequest(new {Message =" Giao dịch không thành công "});
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Đã xảy ra lỗi khi xử lý giao dịch." });
            }
        }












    }
}
