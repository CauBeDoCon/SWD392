namespace SWD392.Models
{
    public class ShippingModel
    {
        public int Id { get; set; }

        public string ShippingAddress { get; set; }

        public DateTime EstimatedDeliveryDate { get; set; }

        public decimal ShippingCost { get; set; }

        public int? ShippingMethodId { get; set; }

        public int? OrderId { get; set; }
    }
}
