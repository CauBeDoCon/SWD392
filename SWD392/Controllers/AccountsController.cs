using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWD392.DB;
using SWD392.DTOs;
using SWD392.Models;
using SWD392.Repositories;
using System.ComponentModel.DataAnnotations;

namespace SWD392.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountRepository accountRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        public AccountsController(IAccountRepository repo, UserManager<ApplicationUser> userManage, ApplicationDbContext context) 
        {
            accountRepo = repo;
            _userManager= userManage;
            _context = context;
        }


        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp([FromBody] SignUpDTO signUpDto)
        {
            if (!new EmailAddressAttribute().IsValid(signUpDto.Email))
            {
                return BadRequest("Email không hợp lệ.");
            }

            var signUpModel = new SignUpModel
            {
                FirstName = signUpDto.FirstName,
                Username = signUpDto.Username,
                LastName = signUpDto.LastName,
                Email = signUpDto.Email,
                Password = signUpDto.Password,
                ConfirmPassword = signUpDto.ConfirmPassword,
                Address = signUpDto.Address,
                Birthday = signUpDto.Birthday,
                PhoneNumber = signUpDto.PhoneNumber
            };

            var result = await accountRepo.SignUpAsync(signUpModel);

            if (result.Succeeded)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == signUpDto.Email);
                if (user == null)
                {
                    return StatusCode(500, new { Message = "Lỗi hệ thống: Không tìm thấy tài khoản sau khi đăng ký!" });
                }

                // ✅ Tạo WalletId ngẫu nhiên nhưng không trùng lặp
                int newWalletId;
                do
                {
                    newWalletId = Math.Abs(Guid.NewGuid().GetHashCode()); // Tạo giá trị ngẫu nhiên
                } while (await _context.Wallets.AnyAsync(w => w.WalletId == newWalletId)); // Kiểm tra trùng lặp

                // ✅ Tạo Wallet mới
                var wallet = new Wallet
                {
                    AmountOfMoney = 0
                };

                _context.Wallets.Add(wallet);
                await _context.SaveChangesAsync();

                // ✅ Lúc này, WalletId đã được tự động sinh, chỉ cần lấy lại từ DB
                user.WalletId = wallet.WalletId;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                Console.WriteLine($"✅ Đã tạo WalletId {wallet.WalletId} cho User {user.Id}");

                return Ok(new { Message = "Đăng ký thành công!" });
            }

            return BadRequest(new { Errors = result.Errors.Select(e => e.Description) });
        }






        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn([FromBody] SignInModel signInModel)
        {
            var result = await accountRepo.SignInAsync(signInModel);

            if (result == null)
            {
                return Unauthorized(new { Message = "Email hoặc mật khẩu không đúng!" });
            }

            return Ok( result);
        }
        [HttpGet("GetAllAccount")]
        public async Task<IActionResult> GetAllAccounts()
        {
            return Ok(await accountRepo.GetAllAccountsAsync());
        }

    }
}
