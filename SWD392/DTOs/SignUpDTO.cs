using System.ComponentModel.DataAnnotations;

namespace SWD392.DTOs
{
    public class SignUpDTO
    {
        [Required(ErrorMessage = "First Name không được để trống")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name không được để trống")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; } = string.Empty; // Đảm bảo không null

        [Required(ErrorMessage = "Password không được để trống")]
        [MinLength(6, ErrorMessage = "Password phải có ít nhất 6 ký tự")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password không được để trống")]
        [Compare("Password", ErrorMessage = "Confirm Password không khớp với Password")]
        public string ConfirmPassword { get; set; }

        public string Address { get; set; }

        [Required(ErrorMessage = "Birthday không được để trống")]
        public DateTime Birthday { get; set; }
    }
}
