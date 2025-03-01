using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWD392.DB
{
    [Table("Review")]
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Rating phải từ 1 đến 5.")]
        public int Rating { get; set; }

        public string? Content { get; set; }

        [Required]
        public DateTime ReviewDate { get; set; } = DateTime.UtcNow;

        [Required]
        public int OrderDetailId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        [ForeignKey("OrderDetailId")]
        public OrderDetail OrderDetail { get; set; }

        public Comment Comment { get; set; }
    }
}
