using System.ComponentModel.DataAnnotations;

namespace SWD392.DTOs
{
    public class UpdateCommentDTO
    {
        [Required]
        public int ReviewId { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
