namespace SWD392.DTOs
{
    public class BookingHistoryDTO
    {
        public int Id { get; set; }

        public int BookingId { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Type { get; set; }

        public string Status { get; set; }
    }
}
