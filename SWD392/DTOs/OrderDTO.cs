namespace SWD392.DTOs
{
    public class OrderDTO
    {
        public int OrderId { get; set; }
        public string UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public int CartId { get; set; }
        //public int? DiscountId { get; set; }
    }

}
