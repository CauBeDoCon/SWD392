using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWD392.DB
{
    [Table("Unit")]
    public class Unit
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        public ICollection<UnitProduct> UnitProducts { get; set; } = new List<UnitProduct>();
    }
}