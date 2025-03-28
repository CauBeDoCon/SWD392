using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SWD392.DB
{
    public class PackageSession
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Package")]
        public int PackageId { get; set; }

        [Required]
        public string? TimeSlot1 { get; set; }
        public string? TimeSlot2 { get; set; }
        public string? TimeSlot3 { get; set; }
        public string? TimeSlot4 { get; set; }

        public string? Description1 { get; set; }
        public string? Description2 { get; set; }
        public string? Description3 { get; set; }
        public string? Description4 { get; set; }
        [JsonIgnore]
        public virtual Package Package { get; set; }
    }
}
