namespace SWD392.DTOs
{
    public class ReviewDTO
    {
        public int Id { get; set; }

        public int Rating { get; set; }

        public string? Content { get; set; }

        public DateTime ReviewDate { get; set; }

        public string UserId { get; set; }

        public int? OrderDetailId { get; set; }
    }
}
