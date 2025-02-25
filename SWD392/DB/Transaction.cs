using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SWD392.DB
{
    [Table("Transaction")]
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }

        public int WalletId { get; set; }

        [Required]
        public double amount { get; set; }

        public DateTime CreatedTransaction { get; set; }

        public string BankName { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string ReasonWithdrawReject { get; set; }

        public string TransactionEnum { get; set; }

        // Navigation Property
        [ForeignKey("WalletId")]
        public Wallet Wallet { get; set; }
    }
}
