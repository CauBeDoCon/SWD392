using Microsoft.AspNetCore.Identity;
using SWD392.DB;
using SWD392.Models;

namespace SWD392.Repositories
{
    public interface IAccountRepository
    {
        public Task<IdentityResult> SignUpAsync(SignUpModel model);
        public Task<object?> SignInAsync(SignInModel model);

        public Task<List<ApplicationUser>> GetAllAccountsAsync();
    }
}
