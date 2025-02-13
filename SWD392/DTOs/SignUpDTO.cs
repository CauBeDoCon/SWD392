namespace SWD392.DTOs
{
    public class SignUpDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Address { get; set; }
        public DateTime Birthday { get; set; }
    }
}
