using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWD392.DB
{
    [Table("UnitProduct")]
    public class UnitProduct
    {
        [Key]
        public int Id { get; set; }
        public int Price { get; set; }
        public int UnitId { get; set; }
        [ForeignKey("UnitId")]
        public Unit Unit { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product products { get; set; }
    }
}
