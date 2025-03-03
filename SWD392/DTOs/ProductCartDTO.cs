namespace SWD392.DTOs
{
    public class ProductCartDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
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
