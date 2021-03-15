using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CSE_Hankers.Models;
using CSE_Hankers.Models.IRepositories;
using Markdig.Syntax;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Westwind.AspNetCore.Markdown;

namespace CSE_Hankers.Controllers
{
    public class ArticleController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly AppDbContext context;
        private readonly IArticleRepository articleRepository;
        private readonly IArticleCommentRepository articleCommentRepository;

        public ArticleController(
            IArticleRepository articleRepository,
            IArticleCommentRepository articleCommentRepository,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            AppDbContext context)
        {
            this.context = context;
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.articleRepository = articleRepository;
            this.articleCommentRepository = articleCommentRepository;
        }

        public ApplicationUser getLoggedUser()
        {
            var userid = signInManager.UserManager.GetUserId(User);
            var user = context.Users.Where(usr => usr.Id == userid).FirstOrDefault();
            return user;
        }

        [Authorize]
        public async Task<IActionResult> Index(
                int? pageNumber)
        {
            IList<Article> articles = articleRepository.GetAllArticles();
            foreach (var article in articles)
            {
                article.author = await userManager.FindByIdAsync(article.authorId);
            }
            var user = getLoggedUser();
            int pageSize = 5;

            return View(PaginatedList<Article>.CreateAsync(articles, pageNumber ?? 1, pageSize));
        }

        [Authorize]
        public async Task<IActionResult> MyArticles(int? pageNumber)
        {
            var user = getLoggedUser();
            var articles = context.Articles.Where(a => a.author == user).ToList();
            foreach (var article in articles)
            {
                article.author = await userManager.FindByIdAsync(article.authorId);
            }
            int pageSize = 5;
            return View(PaginatedList<Article>.CreateAsync(articles, pageNumber ?? 1, pageSize));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Article article)
        {
            if(ModelState.IsValid)
            {
                var user = getLoggedUser();
                article.author = user;
                articleRepository.Add(article);
                TempData["SuccessMessage"] = "Article Published Successfully!";
                return RedirectToAction("MyArticles");
            }
            return View(article);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Edit(int id)
        {
            Article article = articleRepository.GetArticle(id);
            if (article == null)
            {
                TempData["ErrorMessage"] = "Article not found!";
                return RedirectToAction("Index", "Article");
            }
            else
            {
                var user = getLoggedUser();
                if (article.author != user)
                    return RedirectToAction("AccessDenied", "Account");
                return View(article);
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(Article article)
        {
            Article updatedArticle = articleRepository.GetArticle(article.articleId);
            updatedArticle.description = article.description;
            updatedArticle.title = article.title;
            articleRepository.Update(updatedArticle);
            TempData["SuccessMessage"] = "Article Updated Successfully!";
            return RedirectToAction("Index", "Article");
        }


        [HttpPost]
        [Authorize]
        public IActionResult DeleteConfirmed(int id)
        {
            Article article = articleRepository.GetArticle(id);
            if (article == null)
            {
                TempData["ErrorMessage"] = "Article not found!";
                return RedirectToAction("Index", "Article");
            }

            articleRepository.RemoveArticleLikes(article);
            articleCommentRepository.RemoveCommentsOfArticle(article);
            articleRepository.Delete(article);

            TempData["SuccessMessage"] = "Article Deleted Successfully!";
            return RedirectToAction("Index", "Article");
        }


        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            Article article = articleRepository.GetArticle(id);
            var comments = articleCommentRepository.GetComments(article);
            foreach (var comment in comments)
            {
                comment.author = await userManager.FindByIdAsync(comment.authorId);
            }
            var user = getLoggedUser();

            if (article == null)
            {
                TempData["ErrorMessage"] = "Article not found!";
                return RedirectToAction("Index", "Article");
            }
            else
            {
                if (signInManager.IsSignedIn(User))
                {
                    if(article.author == user)
                    {
                        ViewBag.user = user;
                        ViewBag.article = article;
                        ViewBag.comments = comments;
                        return View("ArticleDetails");
                    }
                }
                ViewBag.author = await userManager.FindByIdAsync(article.authorId);
                ViewBag.user = user;
                ViewBag.article = article;
                ViewBag.comments = comments;
                ViewBag.ErrorMessage = "Nooo";
                TempData["ErrorMessage"] = "Article not found!";
                return View();
            }
        }


        [HttpGet]
        [Authorize]
        public IActionResult Like(int id)
        {
            Article article = articleRepository.GetArticle(id);
            var user = getLoggedUser();

            if (article == null)
            {
                TempData["ErrorMessage"] = "Article not found!";
                return View("404");
            }
            else
            {
                var obj = (context.ArticleLikes.Where(al => (al.article == article && al.author==user)));
                if(obj.Count() == 0)
                {
                    var alObject = articleRepository.giveLike(article, user);

                    article.likes = article.likes + 1;
                    articleRepository.Update(article);
                    TempData["SuccessMessage"] = "Article liked!";
                    return RedirectToAction("Details", "Article", new { @id = article.articleId});
                }

                TempData["ErrorMessage"] = "Article already liked!";
                return RedirectToAction("Details", "Article", new { @id = article.articleId });
            }
        }

    }
}
