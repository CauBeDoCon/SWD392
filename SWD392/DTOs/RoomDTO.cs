namespace SWD392.DTOs
{
    public class RoomDTO
    {
        public int RoomId { get; set; }
        public string RoomName { get; set; }
        public int SlotMax { get; set; }
        public int SlotNow { get; set; }
        public string Status { get; set; }
        public string DoctorName { get; set; }
        public string PackageName { get; set; }

    }
}
