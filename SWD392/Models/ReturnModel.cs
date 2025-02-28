namespace SWD392.Models
{
    public class ReturnModel
    {
        public int Id { get; set; }

        public string Reason { get; set; }

        public DateTime ReturnDate { get; set; }

        public string Status { get; set; }

        public int? ShippingId { get; set; }

        public int? OrderId { get; set; }

        public int? ProductId { get; set; }
    }
}
