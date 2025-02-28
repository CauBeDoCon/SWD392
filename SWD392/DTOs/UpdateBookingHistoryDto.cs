namespace SWD392.DTOs
{
    public class UpdateBookingHistoryDto
    {
        public int BookingId { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Type { get; set; }

        public string Status { get; set; }
    }
}
