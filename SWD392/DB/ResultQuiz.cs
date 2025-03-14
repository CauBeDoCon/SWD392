using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SWD392.Enums;
using System.Text.Json.Serialization;
namespace SWD392.DB
{
    public class ResultQuiz
    {
    [Key]
    public int ResultQuizId { get; set; }
    [Required]
    public ResultQuizAnswer Quiz1 { get; set; }
    [Required]
    public ResultQuizAnswer Quiz2 { get; set; }
    [Required]
    public ResultQuizAnswer Quiz3 { get; set; }
    [Required]
    public ResultQuizAnswer Quiz4 { get; set; }
    [Required]
    public ResultQuizAnswer Quiz5 { get; set; }
    [Required]
    public ResultQuizAnswer Quiz6 { get; set; }
    [Required]
    public ResultQuizAnswer Quiz7 { get; set; }
    
    public string UserId { get; set; }
    
    [ForeignKey("UserId")]
    public ApplicationUser User { get; set; }
    
    public DateTime CreateDate { get; set; }
    public int Result { get; set; }
    public SkinType SkinStatus { get; set; }
    public AnceStatus AnceStatus { get; set; }
    
    // Quan hệ với Routine
    [JsonIgnore] // Không serialize thuộc tính này
    public virtual ICollection<Routine> Routines { get; set; } = new List<Routine>();

    }
}