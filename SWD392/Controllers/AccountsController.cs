using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWD392.DB;
using SWD392.DTOs;
using SWD392.Helpers;
using SWD392.Models;
using SWD392.Repositories;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Data;


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
        public async Task<IActionResult> SignUp([FromBody] SignUpDTO signUpDto,string role = AppRole.Customer)
        {
            if (signUpDto.Password.Length < 8)
            {
                return BadRequest(new { Message = "Mật khẩu phải có ít nhất 8 ký tự." });
            }

            if (!new EmailAddressAttribute().IsValid(signUpDto.Email))
            {
                return BadRequest(new { Message = "Email không hợp lệ." });
            }

            var existingUserByEmail = await _userManager.FindByEmailAsync(signUpDto.Email);
            if (existingUserByEmail != null)
            {
                return BadRequest(new { Message = "Email đã tồn tại. Vui lòng sử dụng email khác!" });
            }

            var existingUserByUsername = await _userManager.FindByNameAsync(signUpDto.Username);
            if (existingUserByUsername != null)
            {   
                return BadRequest(new { Message = "Username đã tồn tại. Vui lòng sử dụng Username khác!" });
            }

            var existingUserByPhone = await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == signUpDto.PhoneNumber);
            if (existingUserByPhone != null)
            {
                return BadRequest(new { Message = "Số điện thoại đã tồn tại. Vui lòng sử dụng số khác!" });
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
                PhoneNumber = signUpDto.PhoneNumber,
                Role = role
            };

            var result = await accountRepo.SignUpAsync(signUpModel);

            if (result.Succeeded)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == signUpDto.Email);
                if (user == null)
                {
                    return StatusCode(500, new { Message = "Lỗi hệ thống: Không tìm thấy tài khoản sau khi đăng ký!" });
                }

                int newWalletId;
                do
                {
                    newWalletId = Math.Abs(Guid.NewGuid().GetHashCode()); 
                } while (await _context.Wallets.AnyAsync(w => w.WalletId == newWalletId)); 

                var wallet = new Wallet
                {
                    AmountOfMoney = 0
                };

                _context.Wallets.Add(wallet);
                await _context.SaveChangesAsync();

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
       
        [HttpGet("GetCurrentAccount")]
        public async Task<IActionResult> GetCurrentAccount()
        {
            // Trích xuất userId từ JWT token (từ ClaimTypes.NameIdentifier)
             var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }
            
            // Gọi service để lấy thông tin tài khoản hiện tại theo userId
            var account = await accountRepo.GetAccountByIdAsync(userId);
            if (account == null)
            {
                return NotFound("Không tìm thấy tài khoản.");
            }
            return Ok(account);
        }
        [HttpPut("UpdateAccountInfo")]
        [Authorize]
        public async Task<IActionResult> UpdateAccountInfo([FromBody] UpdateAccountInfo dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }
            
            if (dto == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }
            var result = await accountRepo.UpdateAccountInfoAsync(userId, dto);
            if (result.Succeeded)
            {
                return Ok(new { Message = "Cập nhật tài khoản thành công!" });
            }


            return BadRequest(new { Errors = result.Errors.Select(e => e.Description) });
        }
        [HttpPost("SignUpAdmin")]
        public async Task<IActionResult> SignUpAdmin([FromBody] SignUpDTO signUpDto)
        {
            return await SignUp(signUpDto, AppRole.Admin);
        }

        [HttpPost("SignUpManager")]
        public async Task<IActionResult> SignUpManager([FromBody] SignUpDTO signUpDto)
        {
            return await SignUp(signUpDto, AppRole.Manager);
        }

        [HttpPost("SignUpDoctor")]
        public async Task<IActionResult> SignUpDoctor([FromBody] SignUpDTO signUpDto)
        {
            return await SignUp(signUpDto, AppRole.Doctor);
        }

        [HttpPost("SignUpStaff")]
        public async Task<IActionResult> SignUpStaff([FromBody] SignUpDTO signUpDto)
        {
            return await SignUp(signUpDto, AppRole.Staff);
        }
    }
}
