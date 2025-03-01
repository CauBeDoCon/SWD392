using System.Text.Json.Serialization;

namespace SWD392.Models
{
    public class BlogModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime PublishDate { get; set; } = DateTime.UtcNow;
        public string? Tags { get; set; }
        public string? Image { get; set; }
        [JsonIgnore]
        public string UserId { get; set; }
    }
}
