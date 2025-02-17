using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWD392.DB;
using SWD392.DTOs;
using System.Threading.Tasks;

namespace SWD392.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public WalletController(ApplicationDbContext context)
        {
            _context = context;
        }

      
        [HttpGet("{userId}")]
        public async Task<ActionResult<WalletDTO>> GetWalletBalance(string userId)
        {
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
    }
}
