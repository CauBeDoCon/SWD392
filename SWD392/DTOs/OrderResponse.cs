using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SWD392.DB;

namespace SWD392.DTOs
{
    public class OrderResponse
    {
        public int orderID { get; set; }
        public string  applicationUserID { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public Discount Discount { get; set; }
        public int CartId { get; set; }
        public string Message { get; set; }
        
    }
}