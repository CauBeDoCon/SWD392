using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SWD392.DTOs
{
    public class UpdateAccountInfo
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;
        [DefaultValue("user@example.com")]
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;   
        public string Avatar { get; set; } = string.Empty;
        [Required]
        public string Address { get; set; } = string.Empty;

        public DateTime? Birthday { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống.")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        [RegularExpression(@"^\+?[0-9]{10,15}$", ErrorMessage = "Số điện thoại phải có từ 10 đến 15 chữ số.")]
        public string PhoneNumber { get; set; } = string.Empty;

    }
}