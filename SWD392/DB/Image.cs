using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SWD392.DB
{
    [Table("Image")]
    public class Image
    {
        [Key]
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        [JsonIgnore]
        public Product Product { get; set; }
    }
}