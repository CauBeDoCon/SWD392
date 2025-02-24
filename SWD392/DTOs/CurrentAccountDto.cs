using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWD392.DTOs
{
   // Định nghĩa DTO chỉ chứa các trường cần thiết
public class CurrentAccountDto
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    public string Birthday { get; set; }
    public int? CartId { get; set; }
    public int? WalletId { get; set; }
}

}