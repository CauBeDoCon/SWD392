using SWD392.Enums;

namespace SWD392.DTOs
{
    public class UpdateProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public SkinType skinType { get; set; }  
        public double Price { get; set; }
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
