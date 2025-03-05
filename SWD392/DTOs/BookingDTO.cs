namespace SWD392.DTOs
{
    public class BookingDTO
    {
        public int BookingId { get; set; }
        public DateTime TimeSlot { get; set; }
        public string Status { get; set; }
        public string? CustomerId { get; set; }

        public string? DoctorAvatar { get; set; }
    }
}
