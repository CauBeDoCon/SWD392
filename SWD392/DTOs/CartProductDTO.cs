namespace SWD392.DTOs
{
    public class CartProductDTO
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
        public int CartId { get; set; }
        public ProductCartDTO Product { get; set; } // Lồng toàn bộ thông tin sản phẩm

    }
}
