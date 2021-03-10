using CSE_Hankers.Models.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSE_Hankers.Models.SQLRepositories
{
    public class SQLArticleRepository : IArticleRepository
    {
        private readonly AppDbContext context;

        public SQLArticleRepository(AppDbContext context)
        {
            this.context = context;
        }

        public Article Add(Article article)
        {
            context.Articles.Add(article);
            context.SaveChanges();
            return article;
        }

        public Article Delete(Article Article)
        {
            context.Articles.Remove(Article);
            context.SaveChanges();
            return Article;
        }

        public IList<Article> GetAllArticles()
        {
            return context.Articles.ToList();
        }

        public Article GetArticle(int ArticleID)
        {
            return context.Articles.FirstOrDefault(m => m.articleId == ArticleID);
        }

        public Article GetArticleByUser(ApplicationUser user)
        {
            return context.Articles.Where(a => a.author == user).FirstOrDefault();
        }

        public ArticleLikes giveLike(Article article, ApplicationUser user)
        {
            var alObject = new ArticleLikes()
            {
                article = article,
                author = user,
            };
            context.ArticleLikes.Add(alObject);
            context.SaveChanges();
            return alObject;
        }

        public Article RemoveArticleLikes(Article article)
        {
            var articleLikes = context.ArticleLikes.Where(u => u.article == article);
            context.ArticleLikes.RemoveRange(articleLikes);
            context.SaveChanges();
            return article;
        }

        public Article Update(Article articleChanges)
        {
            var changedArticle = context.Articles.Attach(articleChanges);
            changedArticle.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return articleChanges;
        }
    }
}
