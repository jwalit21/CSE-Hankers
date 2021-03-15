using CSE_Hankers.Models.IRepositories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSE_Hankers.Models.SQLRepositories
{
    public class SQLArticleCommentRepository : IArticleCommentRepository
    {
        private readonly AppDbContext context;

        public SQLArticleCommentRepository(AppDbContext context)
        {
            this.context = context;
        }

        public ArticleComment AddComment(ArticleComment articleComment)
        {
            context.ArticleComments.Add(articleComment);
            context.SaveChanges();
            return articleComment;
        }

        public ArticleComment GetArticleComment(int ArticleCommentId)
        {
            return context.ArticleComments.FirstOrDefault(m => m.articleCommentId == ArticleCommentId);
        }

        public IEnumerable<ArticleComment> GetComments(Article article)
        {
            return context.ArticleComments.Where(p => p.article.articleId == article.articleId).ToList();
        }

        public Article RemoveCommentsOfArticle(Article article)
        {
            var articleComments = context.ArticleComments.Where(o => o.article == article);
            context.ArticleComments.RemoveRange(articleComments);
            context.SaveChanges();
            return article;
        }

        public ArticleComment Update(ArticleComment articleComment)
        {
            var changedArticleComment = context.ArticleComments.Attach(articleComment);
            changedArticleComment.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return articleComment;
        }
    }
}
