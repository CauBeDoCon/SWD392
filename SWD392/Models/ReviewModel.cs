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
        [JsonIgnore]
        public string UserId { get; set; }
    }
}
