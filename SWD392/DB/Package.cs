using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SWD392.DB
{
    public class Package
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Sessions { get; set; }

        public string? DoctorId { get; set; }
        [ForeignKey("DoctorId")]
        public virtual ApplicationUser? Doctor { get; set; }
        [Required]
        public string Status { get; set; } = "inactive";

        [Required]
        public int PackageCount { get; set; }

        [JsonIgnore]
        public virtual ICollection<PackageSession> PackageSessions { get; set; }
        [JsonIgnore]
        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}
