namespace SWD392.DTOs
{
    public class UpdateAccountDto
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public DateTime? Birthday { get; set; }
    }
}
