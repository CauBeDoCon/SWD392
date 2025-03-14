using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SWD392.DB
{
    public class RoutineStep
    {
        [Key]
        public int Id { get; set; }
       // public string Name { get; set; } //Sữa rửa mặt
        public int Step { get; set; }

        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        // Khóa ngoại
        
        public int RoutineId { get; set; }
        [ForeignKey("RoutineId")]
        [JsonIgnore]
        public Routine Routine { get; set; }
        
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        
        public Product Product { get; set; }
    }
}