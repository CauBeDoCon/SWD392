using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SWD392.DB
{
    public class PackageTracking
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("TreatmentSession")]
        public int TreatmentSessionId { get; set; }

        public virtual TreatmentSession TreatmentSession { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string TimeSlot { get; set; }

        [Required]
        public string Status { get; set; } = "not_done";

        public string Description { get; set; }
    }
}
