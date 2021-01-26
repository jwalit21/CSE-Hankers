using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSE_Hankers.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string mobile { get; set; }
        public string organization { get; set; }
        public string photoPath { get; set; }

        public ICollection<Question> questions { get; set; }
        public ICollection<Answer> answers { get; set; }
        public ICollection<Article> articles { get; set; }
        public ICollection<ArticleComment> articleComments { get; set; }

    }
}
