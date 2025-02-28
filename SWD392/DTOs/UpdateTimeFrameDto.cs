namespace SWD392.DTOs
{
    public class UpdateTimeFrameDto
    {
        public string UserId { get; set; }

        public DateTime TimeFrameFrom { get; set; }

        public DateTime TimeFrameTo { get; set; }

        public string Status { get; set; }
    }
}
