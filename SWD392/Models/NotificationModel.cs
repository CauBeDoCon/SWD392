namespace SWD392.Models
{
    public class NotificationModel
    {
        public int Id { get; set; }

        public string Message { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool Status { get; set; }

        public string UserId { get; set; }
    }
}
