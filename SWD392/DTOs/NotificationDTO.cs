namespace SWD392.DTOs
{
    public class NotificationDTO
    {
        public int Id { get; set; }

        public string Message { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool Status { get; set; }

        public string UserId { get; set; }
    }
}
