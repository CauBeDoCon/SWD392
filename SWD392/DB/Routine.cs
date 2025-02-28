using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWD392.DB
{
    [Table("Routine")]
    public class Routine
    {
        [Key]
        public int Id { get; set; }

        public int ResultQuizId { get; set; }

        public int StepOrder { get; set; }

        public string Instruction { get; set; }

        [ForeignKey("ResultQuizId")]
        public ResultQuiz ResultQuiz { get; set; }

        public ICollection<RecommendProduct> RecommendProducts { get; set; } = new List<RecommendProduct>();
    }
}
