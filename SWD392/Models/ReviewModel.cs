namespace SWD392.Models
{
    public class ReviewModel
    {
        public int Id { get; set; }
        public double Rating { get; set; }
        public string Content { get; set; }
        public DateTime ReviewDate { get; set; }
        public string UserId { get; set; }
    }
}
