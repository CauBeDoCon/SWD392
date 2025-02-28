namespace SWD392.Models
{
    public class TimeFrameModel
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public DateTime TimeFrameFrom { get; set; }

        public DateTime TimeFrameTo { get; set; }

        public string Status { get; set; }
    }
}
