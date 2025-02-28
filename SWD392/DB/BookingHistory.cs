using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SWD392.DB
{
    [Table("BookingHistory")]
    public class BookingHistory
    {
        [Key]
        public int Id { get; set; }

        public int BookingId { get; set; }
        [ForeignKey("BookingId")]
        public Booking Booking { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Type { get; set; }

        public string Status { get; set; }
    }
}
