using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using SWD392.Enums;

namespace SWD392.DTOs
{
    public class QuizAnswerUser
    {
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
    }
}