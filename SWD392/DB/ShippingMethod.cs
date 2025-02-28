using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWD392.DB
{
    [Table("ShippingMethod")]
    public class ShippingMethod
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal ShippingCost { get; set; }

        public Shipping Shipping { get; set; }
    }
}
