namespace SWD392.DTOs
{
    public class BookingDTO
    {
        public int BookingId { get; set; }
        public DateTime TimeSlot { get; set; }
        public string Status { get; set; }
        public string? CustomerId { get; set; }

        public string? DoctorAvatar { get; set; }

        public string? DoctorFirstName { get; set; } 
        public string? DoctorLastName { get; set; }

        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerAvatar { get; set; }


    }
}
