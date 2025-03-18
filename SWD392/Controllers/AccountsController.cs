using Microsoft.AspNetCore.Authorization;
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
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace SWD392.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountRepository accountRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IBookingRepository _bookingRepository;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AccountsController(IAccountRepository repo, UserManager<ApplicationUser> userManage, ApplicationDbContext context, IBookingRepository bookingRepository, IConfiguration configuration, SignInManager<ApplicationUser> signInManager)
        {
            accountRepo = repo;
            _userManager = userManage;
            _context = context;
            _bookingRepository = bookingRepository;
            _signInManager = signInManager;
            _configuration = configuration;
        }


        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp([FromBody] SignUpDTO signUpDto, string role = AppRole.Customer)
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

                int newCartId;
                do
                {
                    newCartId = Math.Abs(Guid.NewGuid().GetHashCode());
                } while (await _context.carts.AnyAsync(w => w.Id == newCartId));

                var cart = new Cart { };

                _context.carts.Add(cart);
                await _context.SaveChangesAsync();

                user.CartId = cart.Id;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                Console.WriteLine($"✅ Đã tạo CartId {cart.Id} cho User {user.Id}");

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

            return Ok(result);
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
            var result = await SignUp(signUpDto, AppRole.Doctor);


            if (result is OkObjectResult okResult)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == signUpDto.Email);
                if (user != null)
                {
                    await _bookingRepository.CreateDoctorBookingsAsync(user.Id, 7);
                    Console.WriteLine($"✅ Đã tạo lịch khám cho bác sĩ {user.Email} (DoctorId: {user.Id}) trong 7 ngày.");
                }

            }

            return result;
        }

        [HttpPost("SignUpStaff")]
        public async Task<IActionResult> SignUpStaff([FromBody] SignUpDTO signUpDto)
        {
            return await SignUp(signUpDto, AppRole.Staff);
        }

        [HttpPut("ToggleUserStatus/{userId}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> ToggleUserStatus(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new { Message = "Không tìm thấy người dùng." });
            }


            user.Status = user.Status == "Active" ? "Banned" : "Active";

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return StatusCode(500, new { Message = "Cập nhật trạng thái thất bại." });
            }

            return Ok(new
            {
                Message = $"Người dùng {user.Email} đã được chuyển sang trạng thái {user.Status}.",
                NewStatus = user.Status
            });
        }
            [HttpGet("login-google")]
            public IActionResult LoginWithGoogle()
            {
                var redirectUrl = Url.Action(nameof(HandleGoogleResponse), "Accounts", null, Request.Scheme);
                var properties = _signInManager.ConfigureExternalAuthenticationProperties(GoogleDefaults.AuthenticationScheme, redirectUrl);
                return Challenge(properties, GoogleDefaults.AuthenticationScheme);
            }

            [HttpGet("signin-google")]
            public async Task<IActionResult> HandleGoogleResponse()
            {
                var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
                if (!result.Succeeded)
                    return BadRequest(new { Message = "Google authentication failed" });

                var email = result.Principal.FindFirstValue(ClaimTypes.Email);
                if (string.IsNullOrEmpty(email))
                    return BadRequest(new { Message = "Không thể lấy email từ Google." });

                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    user = new ApplicationUser
                    {
                        UserName = email,
                        Email = email,
                        EmailConfirmed = true
                    };

                    var createResult = await _userManager.CreateAsync(user);
                    if (!createResult.Succeeded)
                    {
                        return BadRequest(new { Message = "Không thể tạo tài khoản mới." });
                    }

                    // Thêm user vào role "Customer"
                    await _userManager.AddToRoleAsync(user, AppRole.Customer);

                    var cart = new Cart();
                    _context.carts.Add(cart);
                    await _context.SaveChangesAsync();
                    user.CartId = cart.Id;

                    var wallet = new Wallet { AmountOfMoney = 0 };
                    _context.Wallets.Add(wallet);
                    await _context.SaveChangesAsync();
                    user.WalletId = wallet.WalletId;

                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                }

                // Lấy danh sách roles của user
                var roles = await _userManager.GetRolesAsync(user);
                var role = roles.FirstOrDefault() ?? AppRole.Customer; // Nếu không có role thì mặc định là Customer

                var token = GenerateJwtToken(user, role);
                return Ok(new { Token = token });
            }

        private string GenerateJwtToken(ApplicationUser user, string role)
        {
            var authClaims = new List<Claim>
     {
         new Claim(ClaimTypes.NameIdentifier, user.Id),
         new Claim(ClaimTypes.Email, user.Email),
         new Claim(ClaimTypes.Role, role),
         new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
     };

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}
