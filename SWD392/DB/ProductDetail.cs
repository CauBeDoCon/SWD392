using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWD392.DB
{
    [Table("ProductDetail")]
    public class ProductDetail
    {
        [Key]
        public int Id { get; set; }
        public string ProductDescription { get; set; }
        public string Ingredient { get; set; }
        public string Effect { get; set; }
        public string HowToUse { get; set; }
        public string SideEffect { get; set; }
        public string Note { get; set; }
        public string Preserve {  get; set; }
        public ICollection<Product> products { get; set; } = new List<Product>();
        
    }
}
