namespace SWD392.DTOs
{
    public class UpdateReturnDto
    {
        public string Reason { get; set; }

        public DateTime ReturnDate { get; set; }

        public string Status { get; set; }

        public int? ShippingId { get; set; }

        public int? OrderId { get; set; }

        public int? ProductId { get; set; }
    }
}
