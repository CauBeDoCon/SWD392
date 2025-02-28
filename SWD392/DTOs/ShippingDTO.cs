namespace SWD392.DTOs
{
    public class ShippingDTO
    {
        public int Id { get; set; }

        public string ShippingAddress { get; set; }

        public DateTime EstimatedDeliveryDate { get; set; }

        public decimal ShippingCost { get; set; }

        public int? ShippingMethodId { get; set; }

        public int? OrderId { get; set; }
    }
}
