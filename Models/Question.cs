﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CSE_Hankers.Models
{
    public class Question
    {
        [Key]
        public int questionId { get; set; }

        [Required]
        [StringLength(20000, ErrorMessage = "Question description should be less than 20000 characters.")]
        [Display(Name = "Question")]
        public string question { get; set; }

        public ApplicationUser author { get; set; }
    }
}
