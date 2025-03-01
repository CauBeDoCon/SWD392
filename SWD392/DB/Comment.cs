using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWD392.DB
{
    [Table("Comment")]
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        public string? Content { get; set; }

        [Required]
        public DateTime CommentDate { get; set; } = DateTime.UtcNow;

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        [Required]
        public int ReviewId { get; set; }

        [ForeignKey("ReviewId")]
        public Review Review { get; set; }
    }
}
