using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SWD392.DTOs
{
    public class AppointmentDTO
    {
        [Required]
        public int PackageId { get; set; }

        [JsonIgnore]
        public DateTime? StartDate { get; set; } 
    }
}
