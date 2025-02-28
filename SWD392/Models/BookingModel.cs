namespace SWD392.Models
{
    public class BookingModel
    {
        public int Id { get; set; }

        public string Account { get; set; }

        public DateTime CreatedAt { get; set; }

        public string UserId { get; set; }

        public string MeetingLink { get; set; }

        public string Status { get; set; }

        public string Type { get; set; }

        public int TimeFrameId { get; set; }
    }
}
