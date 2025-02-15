using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SWD392.DB
{
    [Table("Product")]
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(200)]
        public string Name { get; set; }
        public string Description { get; set; }
        [Range(0, double.MaxValue)]
        public double Price { get; set; }
        [Range(0, 1000)]
        public int Quantity { get; set; }


        public ICollection<Image> Images { get; set; } = new List<Image>();
        public ICollection<UnitProduct> UnitProducts { get; set; } = new List<UnitProduct>();
        public int BrandId { get; set; }
        [ForeignKey("BrandId")]
        public Brand Brand { get; set; }
        public int PackagingId { get; set; }
        [ForeignKey("PackagingId")]
        public Packaging Packaging { get; set; }
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        public int BrandOriginId { get; set; }
        [ForeignKey("BrandOriginId")]
        public BrandOrigin BrandOrigin { get; set; }
        public int ManufacturerId { get; set; }
        [ForeignKey("ManufacturerId")]
        public Manufacturer Manufacturer { get; set; }
        public int ManufacturedCountryId { get; set; }
        [ForeignKey("ManufacturedCountryId")]
        public ManufacturedCountry ManufacturedCountry { get; set; }
        public int ProductDetailId { get; set; }
        [ForeignKey("ProductDetailId")]
        public ProductDetail ProductDetail { get; set; }
    }
}
