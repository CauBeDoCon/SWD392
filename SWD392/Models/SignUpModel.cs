using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SWD392.Models
{
    public class SignUpModel
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string Username { get; set; } = string.Empty;


        [Required]
        public string LastName { get; set; } = string.Empty;
        [DefaultValue("user@example.com")]
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required, Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required]
        public string Address { get; set; } = string.Empty;

        public DateTime? Birthday { get; set; }


        public string Role { get; set; }

        public int? CartId { get; set; }

        public int? WalletId { get; set; }
        [Required(ErrorMessage = "Số điện thoại không được để trống.")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        [RegularExpression(@"^\+?[0-9]{10,15}$", ErrorMessage = "Số điện thoại phải có từ 10 đến 15 chữ số.")]
        public string PhoneNumber { get; set; } = string.Empty;

    }
}
