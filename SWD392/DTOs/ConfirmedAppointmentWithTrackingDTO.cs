namespace SWD392.DTOs
{
    public class ConfirmedAppointmentWithTrackingDTO
    {
        public int AppointmentId { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public string PackageName { get; set; }
        public DateTime StartDate { get; set; }
        public List<PackageTrackingDTO> Trackings { get; set; }
    }
}
