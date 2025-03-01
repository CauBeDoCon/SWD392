namespace SWD392.DTOs
{
    public class CustomerDTO
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public int? WalletId { get; set; }
        public int? CartId { get; set; }

        public string Status { get; set; }
    }
}
