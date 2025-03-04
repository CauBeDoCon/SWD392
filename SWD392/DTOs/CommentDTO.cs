using System.ComponentModel.DataAnnotations;

namespace SWD392.DTOs
{
    public class CommentDTO
    {
        [Required]
        public int ReviewId { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
