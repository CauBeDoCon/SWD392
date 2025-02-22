namespace SWD392.Models
{
    public class CommentModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime ReviewDate { get; set; }
        public int ReviewId { get; set; }
        public string UserId { get; set; }
    }
}
