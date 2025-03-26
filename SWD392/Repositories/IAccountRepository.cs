using Microsoft.AspNetCore.Identity;
using SWD392.DB;
using SWD392.DTOs;
using SWD392.Models;

namespace SWD392.Repositories
{
    public interface IAccountRepository
    {
        public Task<IdentityResult> SignUpAsync(SignUpModel model);
        public Task<object?> SignInAsync(SignInModel model);
        Task<ApplicationUser?> GetUserByUsernameAsync(string username);
        public Task<List<ApplicationUser>> GetAllAccountsAsync();
        public Task<List<ApplicationUser>> GetAllCustomersAsync();
        public Task<ApplicationUser> GetAccountByIdAsync(string id);
        public Task<IdentityResult> UpdateAccountInfoAsync(string id,UpdateAccountInfo updateAccountInfo);
        Task<Dictionary<string, int>> GetUserCountsByRolesAsync();

    }
}
