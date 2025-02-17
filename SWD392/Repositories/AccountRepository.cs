using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SWD392.DB;
using SWD392.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SWD392.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IConfiguration configuration;

        public AccountRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration) 
        { 
            this.userManager= userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
        }
        //public async Task<string?> SignInAsync(SignInModel model)
        //{
        //    var result = await signInManager.PasswordSignInAsync(model.Username, model.Password,false,false);

        //    if (result.IsLockedOut)
        //    {
        //        return null; 
        //    }

        //    if (result.RequiresTwoFactor)
        //    {
        //        return null; 
        //    }

        //    if (!result.Succeeded)
        //    {
        //        return null; 
        //    }


        //    var authClaims = new List<Claim>
        //    {
        //        new Claim(ClaimTypes.Name, model.Username),
        //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())

        //    };

        //    var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));

        //    var token = new JwtSecurityToken(
        //        issuer: configuration["JWT:ValidIssuer"],
        //        audience: configuration["JWT:ValidAudience"],
        //        expires: DateTime.Now.AddMinutes(20),
        //        claims: authClaims,
        //        signingCredentials: new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha512Signature)
        //        );
        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}
        public async Task<object?> SignInAsync(SignInModel model)
        {
            var result = await signInManager.PasswordSignInAsync(model.Username, model.Password, false, false);
            var user = await userManager.FindByNameAsync(model.Username);

            if (user == null || !result.Succeeded)
            {
                return null;
            }

            // 🔹 Lấy danh sách Roles của user
            var roles = await userManager.GetRolesAsync(user);
            Console.WriteLine($"User '{user.UserName}' roles count: {roles.Count}");
            foreach (var role in roles)
            {
                Console.WriteLine($"Role: {role}");
            }
            // 🔹 Tạo token JWT
            var authClaims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

            // 🔹 Thêm roles vào token (nếu có)
            foreach (var role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }
            await userManager.AddToRoleAsync(user, "Customer");
            var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: configuration["JWT:ValidIssuer"],
                audience: configuration["JWT:ValidAudience"],
                expires: DateTime.UtcNow.AddMonths(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha512Signature)
            );

            return new
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                User = new
                {
                    user.Id,
                    user.UserName,
                    user.Email,
                    user.Address,
                    user.Birthday,
                    user.PhoneNumber,
                    user.FirstName,
                    user.LastName,
                    Roles = roles // Trả về danh sách roles
                }
            };
        }

        public async  Task<IdentityResult> SignUpAsync(SignUpModel model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Username,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Address = model.Address,
                Birthday = model.Birthday,
                PhoneNumber = model.PhoneNumber,
                CartId = model.CartId,
                WalletId = model.WalletId
            };
            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                Console.WriteLine("Lỗi khi tạo tài khoản:");
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"- {error.Description}");
                }
            }
            await userManager.AddToRoleAsync(user, "Customer");
            if (!result.Succeeded)
            {
                Console.WriteLine($"❌ Lỗi khi gán role: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
            else
            {
                Console.WriteLine($"✅ Đã gán role 'Customer' cho user '{user.UserName}'");
            }
            return result;

        }
        public async Task<List<ApplicationUser>> GetAllAccountsAsync()
        {
            return await userManager.Users.ToListAsync();
        }
    }
}
