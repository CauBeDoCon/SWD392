namespace SWD392.DTOs
{
    public class CustomerTreatmentScheduleDTO
    {
        public int AppointmentId { get; set; }

        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public string PackageName { get; set; }
        public DateTime StartDate { get; set; }
        public string Status { get; set; }

        public string DoctorId { get; set; }
        public string DoctorName { get; set; }
        public string DoctorAvatar { get; set; }
        public string DoctorPhone { get; set; } 

        public List<TreatmentSessionDTO> TreatmentSessions { get; set; }
    }
}
