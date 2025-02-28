using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SWD392.Enums;

namespace SWD392.DB
{
    [Table("CartProduct")]
    public class CartProduct
    {
        [Key]
        public int Id { get; set; }

        public int Quantity { get; set; }

        public CartStatus Status { get; set; }

        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        public int CartId { get; set; }
        [ForeignKey("CartId")]
        public Cart Cart { get; set; }
    }
}
