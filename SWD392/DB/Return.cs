using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SWD392.DB
{
    [Table("Return")]
    public class Return
    {
        [Key]
        public int Id { get; set; }

        public string Reason { get; set; }

        public DateTime ReturnDate { get; set; }

        public string Status { get; set; }

        public int? ShippingId { get; set; }

        [ForeignKey("ShippingId")]
        public Shipping Shipping { get; set; }

        public int? OrderId { get; set; }

        [ForeignKey("OrderId")]
        public Order Order { get; set; }

        public int? ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

    }
}
