using System.ComponentModel.DataAnnotations;

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

        public int UnitId { get; set; }

        public int BrandId { get; set; }

        public int PackagingId { get; set; }

        public int CategoryId { get; set; }

        public int BrandOriginId { get; set; }

        public int ManufacturerId { get; set; }

        public int ManufacturedCountryId { get; set; }

        public int ProductDetailId { get; set; }
    }
}