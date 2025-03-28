namespace SWD392.DTOs
{
    public class PackageWithDoctorDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Sessions { get; set; }
        public string DoctorId { get; set; } 
        public string DoctorName { get; set; }
        public string DoctorAvatar { get; set; }
        public int PackageCount { get; set; }
        public string Status { get; set; }
    }
}
