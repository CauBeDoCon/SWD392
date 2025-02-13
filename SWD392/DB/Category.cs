using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWD392.DB
{
    [Table("Category")]
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        public string Image { get; set; }
        public ICollection<Product> products { get; set; } = new List<Product>();
        public int SolutionId { get; set; }
        [ForeignKey("SolutionId")]
        public Solution Solution { get; set; }
    }
}
