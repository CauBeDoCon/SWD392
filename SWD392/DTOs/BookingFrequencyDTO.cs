using System.Collections.Generic;

namespace SWD392.DTOs
{
    public class BookingFrequencyDTO
    {
        public List<string> Labels { get; set; } = new List<string>();
        public List<int> Data { get; set; } = new List<int>();
    }
}
