using Microsoft.AspNetCore.Identity;
using SWD392.Models;

namespace SWD392.Repositories
{
    public interface IAccountRepository
    {
        public Task<IdentityResult> SignUpAsync(SignUpModel model);
        public Task<string?> SignInAsync(SignInModel model);
    }
}
