using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSE_Hankers.Models.ViewModels;

namespace CSE_Hankers.Models
{
    public class AppDbContext: IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<LoginModel>().HasNoKey();
        }

        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<ArticleComment> ArticleComments { get; set; }
        public DbSet<ArticleLikes> ArticleLikes { get; set; }
        public DbSet<UserFollowing> UserFollowings { get; set; }
    }
}
