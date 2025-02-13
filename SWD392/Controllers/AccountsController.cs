using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWD392.DTOs;
using SWD392.Models;
using SWD392.Repositories;

namespace SWD392.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountRepository accountRepo;

        public AccountsController(IAccountRepository repo) 
        {
            accountRepo = repo;

        }
        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp(SignUpDTO signUpDto)
        {
            var signUpModel = new SignUpModel
            {
                FirstName = signUpDto.FirstName,
                LastName = signUpDto.LastName,
                Email = signUpDto.Email,
                Password = signUpDto.Password,
                ConfirmPassword = signUpDto.ConfirmPassword,
                Address = signUpDto.Address,
                Birthday = signUpDto.Birthday
            };

            var result = await accountRepo.SignUpAsync(signUpModel);

            if (result.Succeeded)
            {
                return Ok(new { Message = "Đăng ký thành công!" });
            }

            return BadRequest(new { Errors = result.Errors.Select(e => e.Description) });
        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(SignInModel signInModel)
        {
            var result = await accountRepo.SignInAsync(signInModel);
            if (result == null)
            {
                return Unauthorized(new { Message = "Email hoặc mật khẩu không đúng!" });
            }

            return Ok(new { Token = result, Message = "Đăng nhập thành công!" });
        }
    }
}
