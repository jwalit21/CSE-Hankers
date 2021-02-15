using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CSE_Hankers.Models
{
    public class ArticleLikes
    {
        [Key]
        public int articleLikeaId { get; set; }

        public Article article { get; set; }

        public ApplicationUser author { get; set; }
    }
}
