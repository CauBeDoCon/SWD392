using System.ComponentModel.DataAnnotations;

namespace SWD392.DTOs
{
    public class ReviewDTO
    {
        [Required]
        [Range(1, 5, ErrorMessage = "Rating phải từ 1 đến 5.")]
        public int Rating { get; set; }

        public string? Content { get; set; }
    }
}
