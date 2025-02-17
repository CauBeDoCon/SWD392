namespace SWD392.DTOs
{
    public class TransactionDTO
    {
        public int TransactionId { get; set; }
        public int WalletId { get; set; }
        public string Account { get; set; }
        public DateTime CreatedTransaction { get; set; }
        public string BankName { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string ReasonWithdrawReject { get; set; }
        public string TransactionEnum { get; set; }
    }

}
