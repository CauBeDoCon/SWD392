﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        public AccountsController(IAccountRepository repo, UserManager<ApplicationUser> userManage) 
        {
            accountRepo = repo;
            _userManager= userManage;
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
                Username= signUpDto.Username,
                LastName = signUpDto.LastName,
                Email = signUpDto.Email,
                Password = signUpDto.Password,
                ConfirmPassword = signUpDto.ConfirmPassword,
                Address = signUpDto.Address,
                Birthday = signUpDto.Birthday,
                PhoneNumber= signUpDto.PhoneNumber
            };

            var result = await accountRepo.SignUpAsync(signUpModel);

            if (result.Succeeded)
            {
                
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
