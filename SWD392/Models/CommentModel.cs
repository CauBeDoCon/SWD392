using System;
using System.Text.Json.Serialization;

namespace SWD392.Model
{
    public class CommentModel
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public DateTime CommentDate { get; set; }
        public int ReviewId { get; set; }
        [JsonIgnore]
        public string UserId { get; set; }
    }
}
