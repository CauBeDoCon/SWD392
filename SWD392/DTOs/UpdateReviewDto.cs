namespace SWD392.DTOs
{
    public class UpdateReviewDto
    {
        public int Rating { get; set; }

        public string? Content { get; set; }

        public DateTime ReviewDate { get; set; }

        public string UserId { get; set; }

        public int? OrderDetailId { get; set; }
    }
}
