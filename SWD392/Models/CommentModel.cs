using SWD392.DB;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SWD392.Models
{
    public class CommentModel
    {
        public int Id { get; set; }

        public string? Content { get; set; }

        public DateTime CommentDate { get; set; }

        public string UserId { get; set; }

        public int? ReviewId { get; set; }
    }
}
