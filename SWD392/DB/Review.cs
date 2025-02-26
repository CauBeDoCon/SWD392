using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWD392.DB
{
    [Table("Review")]
    public class Review
    {
        [Key]
        public int Id { get; set; }
        public double Rating { get; set; }
        public string Content { get; set; }
        public DateTime ReviewDate { get; set; }
        // Navigation Properties
        public Comment Comment { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        public int OrderDetailId { get; set; }
        [ForeignKey("OrderDetailId")]
        public OrderDetail OrderDetail { get; set; }

    }
}
