namespace SWD392.DTOs
{
    public class CreateRoomDTO
    {
        public string RoomName { get; set; }
        public int SlotMax { get; set; }
        public string DoctorId { get; set; }
        public int PackageId { get; set; }
        public string PackageName { get; set; }
    }
}
