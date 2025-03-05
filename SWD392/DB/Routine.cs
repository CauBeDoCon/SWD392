using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using SWD392.Enums;

namespace SWD392.DB
{
    public class Routine
    {
    [Key]
    
    public int Id { get; set; }

    public RoutineCategoryType RoutineCategory { get; set; }
    
    public int ResultQuizId { get; set; }
    [ForeignKey("ResultQuizId")]
    public  ResultQuiz ResultQuiz { get; set; }

    public  ICollection<RoutineStep> routineSteps { get; set; } = new List<RoutineStep>();
    }
}