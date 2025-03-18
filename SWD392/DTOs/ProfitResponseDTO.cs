    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    namespace SWD392.DTOs
    {
        public class ProfitResponseDTO
        {
            public int month { get; set; }
            public decimal revenuePortal { get; set; }
            public List<OrderCheckDto> orderCheckDtos { get; set; } 
        }
    }