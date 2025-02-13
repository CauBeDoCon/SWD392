using System.ComponentModel.DataAnnotations;

namespace SWD392.DTOs
{
    public class SignUpDTO
    {
        [Required(ErrorMessage = "Username không được để trống.")]
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email không được để trống.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Mật khẩu không được để trống.")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Vui lòng xác nhận mật khẩu.")]
        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không khớp.")]
        public string ConfirmPassword { get; set; }
        public string Address { get; set; }
        public DateTime? Birthday { get; set; }
        [Required(ErrorMessage = "Số điện thoại không được để trống.")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        [RegularExpression(@"^\+?[0-9]{10,15}$", ErrorMessage = "Số điện thoại phải có từ 10 đến 15 chữ số.")]
        public string PhoneNumber { get; set; }
    }
}
