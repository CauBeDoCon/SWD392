using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWD392.DTOs
{
    public class OrderProcessResult
    {
         public bool Success { get; set; }
        public string Message { get; set; }
        public int? OrderId { get; set; }
    }
}