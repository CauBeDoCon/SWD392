using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWD392.DTOs
{
    public class OrderCheckDto
    {
        public int orderID { get; set; }
        public string  applicationUserID { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? CanceledDate { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsRefunded { get; set; }
        public string Status { get; set; }
        public int DiscountId { get; set; }
        public int CartId { get; set; }
    }
}