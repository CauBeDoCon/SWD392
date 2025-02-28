using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWD392.DB
{
    [Table("RecommendProduct")]
    public class RecommendProduct
    {
        [Key]
        public int Id { get; set; }

        public int ProductId { get; set; }

        public string RecommendReason { get; set; }

        public int RoutineId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [ForeignKey("RoutineId")]
        public Routine Routine { get; set; }
    }
}
