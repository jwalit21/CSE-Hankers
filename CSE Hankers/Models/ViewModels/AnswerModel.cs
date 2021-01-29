using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CSE_Hankers.Models.ViewModels
{
    public class AnswerModel
    {
        [Key]
        public int answerId { get; set; }

        [Required]
        [StringLength(20000, ErrorMessage = "Description should be less than 20000 characters.")]
        [Display(Name = "Description of the Article")]
        public string description { get; set; }
    }
}
