using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SWD392.DB
{
    [Table("Wallet")]
    public class Wallet
    {
        [Key]
        public int WalletId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal AmountOfMoney { get; set; }

        public ApplicationUser User { get; set; }
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
