namespace SWD392.DTOs
{
    public class BookingResultDTO
    {
        public int Id { get; set; }

        public int BookingId { get; set; }

        public DateTime CreatedAt { get; set; }

        public string ResultsOfDoctor { get; set; }

        public string Status { get; set; }

        public string StatusSkin { get; set; }

        public string StatusAcne { get; set; }
    }
}
