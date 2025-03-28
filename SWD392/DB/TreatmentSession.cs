using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SWD392.DB
{
    public class TreatmentSession
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Appointment")]
        public int AppointmentId { get; set; }

        public virtual Appointment Appointment { get; set; }

        [Required]
        public int SessionNumber { get; set; } // Số buổi trong gói

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string TimeSlot { get; set; }

        [Required]
        public string Status { get; set; } = "scheduled";

        public string Description { get; set; }

        public virtual ICollection<PackageTracking> PackageTrackings { get; set; }
    }
}
