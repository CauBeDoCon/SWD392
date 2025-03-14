using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWD392.DTOs
{
    public class ProductwithImageDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public String Description { get; set; }
        public List<UpdateImageDto> Images { get; set; }
    }
}