using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSE_Hankers.Models.IRepositories
{
    public interface IArticleRepository
    {
        Article Add(Article article);
        Article GetArticle(int ArticleID);
        Article GetArticleByUser(ApplicationUser user);
        IList<Article> GetAllArticles();
        Article Update(Article articleChanges);
        Article Delete(Article Article);
        Article RemoveArticleLikes(Article article);
        ArticleLikes giveLike(Article article, ApplicationUser user);
    }
}
