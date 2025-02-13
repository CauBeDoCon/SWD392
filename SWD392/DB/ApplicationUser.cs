using Microsoft.AspNetCore.Identity;

namespace SWD392.DB
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public DateTime? Birthday { get; set; }

    }
}
