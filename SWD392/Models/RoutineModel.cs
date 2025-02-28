namespace SWD392.Models
{
    public class RoutineModel
    {
        public int Id { get; set; }

        public int ResultQuizId { get; set; }

        public int StepOrder { get; set; }

        public string Instruction { get; set; }
    }
}
