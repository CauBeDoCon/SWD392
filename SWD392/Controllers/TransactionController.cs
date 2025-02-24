using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWD392.DB;
using SWD392.DTOs;
using System.Collections.Generic;
using System.Linq;
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

        [HttpPost]
        public async Task<ActionResult<TransactionDTO>> CreateTransaction(TransactionDTO transactionDto)
        {
            var transaction = new Transaction
            {
                WalletId = transactionDto.WalletId,
                Account = transactionDto.Account,
                CreatedTransaction = transactionDto.CreatedTransaction,
                BankName = transactionDto.BankName,
                AccountName = transactionDto.AccountName,
                AccountNumber = transactionDto.AccountNumber,
                ReasonWithdrawReject = transactionDto.ReasonWithdrawReject,
                TransactionEnum = transactionDto.TransactionEnum
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTransaction), new { id = transaction.TransactionId }, transactionDto);
        }
    }
}
