using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace SWD392.DB
{
    [Table("TimeFrame")]
    public class TimeFrame
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        public DateTime TimeFrameFrom { get; set; }

        public DateTime TimeFrameTo { get; set; }

        public string Status { get; set; }

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
