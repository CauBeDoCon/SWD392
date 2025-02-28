using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SWD392.DB
{
    [Table("Notification")]
    public class Notification
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Message { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool Status { get; set; }

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
    }
}
