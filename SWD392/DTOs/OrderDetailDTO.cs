namespace SWD392.DTOs
{
    public class OrderDetailDTO
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Subtotal { get; set; }
    }

}
