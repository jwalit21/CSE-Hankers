using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSE_Hankers.Models.IRepositories
{
    public interface IArticleCommentRepository
    {
        IEnumerable<ArticleComment> GetComments(Article article);
        ArticleComment GetArticleComment(int ArticleCommentId);
        ArticleComment AddComment(ArticleComment articleComment);
        ArticleComment Update(ArticleComment articleComment);
        Article RemoveCommentsOfArticle(Article article);
    }
}
