using System.Text.Json.Serialization;

namespace SWD392.Models
{
    public class ReviewModel
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string? Content { get; set; }
        public DateTime ReviewDate { get; set; }
        public int OrderDetailId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Avatar { get; set; }
    }
}
