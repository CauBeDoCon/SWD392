namespace SWD392.DTOs
{
    public class UpdateCommentDto
    {
        public string? Content { get; set; }

        public DateTime CommentDate { get; set; }

        public string UserId { get; set; }

        public int? ReviewId { get; set; }
    }
}
