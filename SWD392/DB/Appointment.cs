using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SWD392.DB
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        [Required]
        [ForeignKey("Package")]
        public int PackageId { get; set; }

        public virtual Package Package { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public string Status { get; set; } = "confirmed";

        public virtual ICollection<TreatmentSession> TreatmentSessions { get; set; }
    }
}
