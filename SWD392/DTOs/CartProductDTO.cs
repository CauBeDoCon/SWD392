namespace SWD392.DTOs
{
    public class CartProductDTO
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } // Thêm tên sản phẩm
        public double Price { get; set; } // Thêm giá sản phẩm
    }
}
