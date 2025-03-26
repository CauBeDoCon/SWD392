namespace SWD392.DB
{
    public class Room
    {
        public int RoomId { get; set; }
        public string RoomName { get; set; }
        public string TimeSlot { get; set; }
        public int SlotMax { get; set; }
        public int SlotNow { get; set; }
        public string Status { get; set; } = "Available";
        public string DoctorId { get; set; }
        public DateTime CheckinTime { get; set; }
        public int PackageId { get; set; }
        public Package Package { get; set; }

        public ApplicationUser Doctor { get; set; }
        public ICollection<RoomCheckin> RoomCheckins { get; set; }
    }
}
