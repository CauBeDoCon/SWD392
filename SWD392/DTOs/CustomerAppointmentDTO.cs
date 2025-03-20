namespace SWD392.DTOs
{
    public class CustomerAppointmentDTO
    {
        public string PackageName { get; set; }
        public DateTime StartDate { get; set; }
        public string Status { get; set; }
        public List<TreatmentSessionDTO> TreatmentSessions { get; set; }
    }
}
