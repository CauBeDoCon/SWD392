namespace SWD392.DTOs
{
    public class WithdrawRequestDTO
    {
        public decimal Amount { get; set; }
        public string BankName { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
    }
}
