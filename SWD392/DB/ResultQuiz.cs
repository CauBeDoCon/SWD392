using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWD392.DB
{
    [Table("ResultQuiz")]
    public class ResultQuiz
    {
        [Key]
        public int Id { get; set; }

        public string Quiz1 { get; set; }
        public string Quiz2 { get; set; }
        public string Quiz3 { get; set; }
        public string Quiz4 { get; set; }
        public string Quiz5 { get; set; }
        public string Quiz6 { get; set; }
        public string Result { get; set; }
        public string SkinStatus { get; set; }

        public string Quiz7 { get; set; }
        public string AcneStatus { get; set; }

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        public ICollection<Routine> Routines { get; set; } = new List<Routine>();
    }
}
