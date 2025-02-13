using System.ComponentModel.DataAnnotations;

namespace SWD392.Models
{
    public class SignUpModel
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required, Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required]
        public string Address { get; set; } = string.Empty;

        public DateTime? Birthday { get; set; }

        public int RoleId { get; set; } = 2; 

        public int CartId { get; set; }

        public int WalletId { get; set; }
    }
}
