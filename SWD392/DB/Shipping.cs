using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWD392.DB
{
    [Table("Shipping")]
    public class Shipping
    {
        [Key]
        public int Id { get; set; }

        public string ShippingAddress { get; set; }

        public DateTime EstimatedDeliveryDate { get; set; }

        public decimal ShippingCost { get; set; }

        public int? ShippingMethodId { get; set; }

        [ForeignKey("ShippingMethodId")]
        public ShippingMethod ShippingMethod { get; set; }

        public int? OrderId { get; set; }

        [ForeignKey("OrderId")]
        public Order Order { get; set; }

        public Return Return { get; set; }
    }
}
