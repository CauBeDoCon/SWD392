namespace SWD392.DTOs
{
    public class ReviewDTO
    {
        public int Id { get; set; }
        public double Rating { get; set; }
        public string Content { get; set; }
        public DateTime ReviewDate { get; set; }
        public string UserId { get; set; }
    }
}
