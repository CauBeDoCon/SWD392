using System.ComponentModel.DataAnnotations;
using SWD392.DB;

namespace SWD392.Models
{
    public class ProductModel
    {
        public int Id { get; set; }

        [MaxLength(200)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Range(0, double.MaxValue)]
        public double Price { get; set; }

        [Range(0, 1000)]
        public int Quantity { get; set; }

        public required UnitModel Unit { get; set; }

        public required BrandModel Brand { get; set; }

        public required PackagingModel Packaging { get; set; }

        public required CategoryModel Category { get; set; }

        public required BrandOriginModel BrandOrigin { get; set; }

        public required ManufacturerModel Manufacturer { get; set; }

        public required ManufacturedCountryModel ManufacturedCountry { get; set; }

        public required ProductDetailModel ProductDetail { get; set; }
    }
}