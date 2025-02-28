namespace SWD392.DTOs
{
    public class UpdateBlogDto
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime PublishDate { get; set; }

        public string? Tags { get; set; }

        public string? Image { get; set; }

        public string UserId { get; set; }
    }
}
