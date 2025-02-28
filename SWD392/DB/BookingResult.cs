using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWD392.DB
{
    [Table("BookingResult")]
    public class BookingResult
    {
        [Key]
        public int Id { get; set; }

        public int BookingId { get; set; }
        [ForeignKey("BookingId")]
        public Booking Booking { get; set; }

        public DateTime CreatedAt { get; set; }

        public string ResultsOfDoctor { get; set; }

        public string Status { get; set; }

        public string StatusSkin { get; set; }

        public string StatusAcne { get; set; }
    }
}
