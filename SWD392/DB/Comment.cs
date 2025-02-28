using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SWD392.DB
{
    [Table("Comment")]
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        public string? Content { get; set; }

        public DateTime CommentDate { get; set; }

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        public int? ReviewId { get; set; }

        [ForeignKey("ReviewId")]
        public Review Review { get; set; }
    }
}
