using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWD392.DTOs
{
    public class DiscountDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public decimal Percentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public int max_usage { get; set; }
        public int DiscountCategoryId { get; set; }
       
    }
}