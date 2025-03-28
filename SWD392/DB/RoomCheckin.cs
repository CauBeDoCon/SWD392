namespace SWD392.DB
{
    public class RoomCheckin
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public int AppointmentId { get; set; }
        public string CustomerId { get; set; }
        public DateTime CheckinTime { get; set; }

        public Room Room { get; set; }
        public Appointment Appointment { get; set; }
        public ApplicationUser Customer { get; set; }
    }
}
