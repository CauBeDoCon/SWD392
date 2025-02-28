using SWD392.DB;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SWD392.Models
{
    public class ReviewModel
    {
        public int Id { get; set; }

        public int Rating { get; set; }

        public string? Content { get; set; }

        public DateTime ReviewDate { get; set; }

        public string UserId { get; set; }

        public int? OrderDetailId { get; set; }
    }
}
