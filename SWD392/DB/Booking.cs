using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWD392.DB
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        [Required]
        public string DoctorId { get; set; } 

        public string? CustomerId { get; set; } 

        [Required]
        public DateTime TimeSlot { get; set; }

        [Required]
        public string Status { get; set; } = "Available";
    }
}
