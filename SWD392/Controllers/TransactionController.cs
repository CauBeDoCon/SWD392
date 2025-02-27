using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWD392.DB;
using SWD392.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SWD392.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TransactionController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionDTO>>> GetTransactions()
        {
            return await _context.Transactions
                .Select(t => new TransactionDTO
                {
                    TransactionId = t.TransactionId,
                    WalletId = t.WalletId,
                    Account = t.Account,
                    CreatedTransaction = t.CreatedTransaction,
                    BankName = t.BankName,
                    AccountName = t.AccountName,
                    AccountNumber = t.AccountNumber,
                    ReasonWithdrawReject = t.ReasonWithdrawReject,
                    TransactionEnum = t.TransactionEnum
                })
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionDTO>> GetTransaction(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);

            if (transaction == null)
            {
                return NotFound();
            }

            return new TransactionDTO
            {
                TransactionId = transaction.TransactionId,
                WalletId = transaction.WalletId,
                Account = transaction.Account,
                CreatedTransaction = transaction.CreatedTransaction,
                BankName = transaction.BankName,
                AccountName = transaction.AccountName,
                AccountNumber = transaction.AccountNumber,
                ReasonWithdrawReject = transaction.ReasonWithdrawReject,
                TransactionEnum = transaction.TransactionEnum
            };
        }

        [HttpPost("Withdraw")]
        public async Task<ActionResult<TransactionDTO>> CreateWithdrawRequest([FromBody] WithdrawRequestDTO request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Bạn chưa đăng nhập hoặc token không hợp lệ.");
            }

            var user = await _context.Users
                .Include(u => u.Wallet)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null || user.Wallet == null)
            {
                return NotFound("Người dùng không tồn tại hoặc chưa có ví.");
            }

            if (request.Amount <= 0 || request.Amount > user.Wallet.AmountOfMoney)
            {
                return BadRequest("Số tiền rút không hợp lệ.");
            }

            var transaction = new Transaction
            {
                WalletId = user.Wallet.WalletId,
                Account = user.Email,
                CreatedTransaction = DateTime.UtcNow,
                BankName = request.BankName,
                AccountName = request.AccountName,
                AccountNumber = request.AccountNumber,
                Type = "Withdraw",
                TransactionEnum = "Pending",  
                ReasonWithdrawReject = string.Empty,
                Amount = request.Amount
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTransaction), new { id = transaction.TransactionId }, new TransactionDTO
            {
                TransactionId = transaction.TransactionId,
                WalletId = transaction.WalletId,
                Account = transaction.Account,
                CreatedTransaction = transaction.CreatedTransaction,
                BankName = transaction.BankName,
                AccountName = transaction.AccountName,
                AccountNumber = transaction.AccountNumber,
                TransactionEnum = transaction.TransactionEnum,
                ReasonWithdrawReject = transaction.ReasonWithdrawReject
            });
        }

        [Authorize(Roles = "Admin,Manager")]
        [HttpPut("ApproveTransaction/{id}")]
        public async Task<IActionResult> ApproveTransaction(int id, [FromBody] ApproveTransactionDTO request)
        {
            var transaction = await _context.Transactions
                .FirstOrDefaultAsync(t => t.TransactionId == id);

            if (transaction == null)
            {
                return NotFound("Không tìm thấy giao dịch.");
            }

            if (transaction.TransactionEnum != "Pending")
            {
                return BadRequest("Giao dịch này đã được xử lý trước đó.");
            }

            var userWallet = await _context.Wallets
                .FirstOrDefaultAsync(w => w.WalletId == transaction.WalletId);

            if (userWallet == null)
            {
                return NotFound("Ví của người dùng không tồn tại.");
            }

            decimal amountToWithdraw = transaction.Amount;  

            if (request.IsApproved)
            {
                if (userWallet.AmountOfMoney < amountToWithdraw)
                {
                    return BadRequest("Số dư không đủ để thực hiện rút tiền.");
                }

          
                userWallet.AmountOfMoney -= amountToWithdraw;
                transaction.TransactionEnum = "Accepted";
            }
            else
            {
     
                transaction.TransactionEnum = "Rejected";
                transaction.ReasonWithdrawReject = request.ReasonReject;
            }
            transaction.Type = "Withdraw";
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = request.IsApproved ? "Giao dịch đã được chấp nhận và trừ tiền trong ví." : "Giao dịch đã bị từ chối.",
                TransactionStatus = transaction.TransactionEnum,
                Type = transaction.Type,
                NewBalance = userWallet.AmountOfMoney
            });
        }


        [HttpGet("UserTransactions")]
        public async Task<ActionResult<IEnumerable<TransactionDTO>>> GetUserTransactions()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Bạn chưa đăng nhập hoặc token không hợp lệ.");
            }

            var user = await _context.Users
                .Include(u => u.Wallet)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null || user.Wallet == null)
            {
                return NotFound("Người dùng không tồn tại hoặc chưa có ví.");
            }

            var transactions = await _context.Transactions
                .Where(t => t.WalletId == user.Wallet.WalletId)
                .OrderByDescending(t => t.CreatedTransaction)
                .Select(t => new TransactionDTO
                {
                    TransactionId = t.TransactionId,
                    WalletId = t.WalletId,
                    Account = t.Account,
                    CreatedTransaction = t.CreatedTransaction,
                    BankName = t.BankName,
                    AccountName = t.AccountName,
                    AccountNumber = t.AccountNumber,
                    ReasonWithdrawReject = t.ReasonWithdrawReject,
                    TransactionEnum = t.TransactionEnum
                })
                .ToListAsync();

            return Ok(transactions);
        }

        [Authorize(Roles = "Admin,Manager")]
        [HttpGet("AllTransactions")]
        public async Task<ActionResult<IEnumerable<TransactionDTO>>> GetAllTransactions()
        {
            var transactions = await _context.Transactions
                .OrderByDescending(t => t.CreatedTransaction)
                .Select(t => new TransactionDTO
                {
                    TransactionId = t.TransactionId,
                    WalletId = t.WalletId,
                    Account = t.Account,
                    CreatedTransaction = t.CreatedTransaction,
                    BankName = t.BankName,
                    AccountName = t.AccountName,
                    AccountNumber = t.AccountNumber,
                    ReasonWithdrawReject = t.ReasonWithdrawReject,
                    TransactionEnum = t.TransactionEnum
                })
                .ToListAsync();

            return Ok(transactions);
        }


    }
}
