using SWD392.Enums;

namespace SWD392.Models
{
    public class CartProductModel
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public CartStatus Status { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }
    }
}
