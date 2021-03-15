using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CSE_Hankers.Models
{
    public class ArticleComment
    {
        [Key]
        public int articleCommentId { get; set; }

        [Required]
        [StringLength(10000, ErrorMessage = "Comment should be less than 10000 characters.")]
        [Display(Name = "Comment on the Article")]
        public string comment { get; set; }

        public int likes { get; set; } = 0;

        public int articleId { get; set; }

        public Article article { get; set; }

        public string authorId { get; set; }

        public ApplicationUser author { get; set; }
    }
}
