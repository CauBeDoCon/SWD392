using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace SWD392.DB
{
    [Table("Booking")]
    public class Booking
    {
        [Key]
        public int Id { get; set; }

        public string Account { get; set; }

        public DateTime CreatedAt { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        public string MeetingLink { get; set; }

        public string Status { get; set; }

        public string Type { get; set; }

        public int TimeFrameId { get; set; }
        [ForeignKey("TimeFrameId")]
        public TimeFrame TimeFrame { get; set; }

        public ICollection<BookingHistory> BookingHistories { get; set; } = new List<BookingHistory>();

        public BookingResult BookingResult { get; set; }
    }
}
