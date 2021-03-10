using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSE_Hankers.Models;
using CSE_Hankers.Models.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CSE_Hankers.Controllers
{
    public class ArticleCommentController : Controller
    {

        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly AppDbContext context;
        private readonly IArticleRepository articleRepository;
        private readonly IArticleCommentRepository articleCommentRepository;

        public ArticleCommentController(
            IArticleCommentRepository articleCommentRepository,
            IArticleRepository articleRepository,
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

        [HttpGet]
        public IActionResult Comments(int id)
        {
            Article article = articleRepository.GetArticle(id);
            if (article == null)
            {
                TempData["ErrorMessage"] = "Article not found!";
                return View("404");
            }

            var user = getLoggedUser();
            var comments = articleCommentRepository.GetComments(article);
            ViewBag.id = id;
            ViewBag.user = user;
            return View(comments);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create(int id)
        {
            Article article = articleRepository.GetArticle(id);
            if (article == null)
            {
                TempData["ErrorMessage"] = "Article not found!";
                return View("404");
            }
            ViewBag.id = id;
            return View();
        }

        [HttpPost]
        public IActionResult Create(ArticleComment articleComment)
        {
            if (ModelState.IsValid)
            {
                int articleId = articleComment.likes; // likes gives us the articleId coming from the View
                var user = getLoggedUser();

                Article article = articleRepository.GetArticle(articleId);
                if (article == null)
                {
                    TempData["ErrorMessage"] = "Article not found!";
                    return View("404");
                }

                articleComment.author = user; articleComment.likes = 0; articleComment.article = article;
                articleCommentRepository.AddComment(articleComment);
                TempData["SuccessMessage"] = "comment added Successfully!";
                return RedirectToAction("Comments","ArticleComment",articleId);
            }
            return View(articleComment);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Edit(int id)
        {
            ArticleComment articleComment = articleCommentRepository.GetArticleComment(id);
            if (articleComment == null)
            {
                TempData["ErrorMessage"] = "Comment not found!";
                return View("404");
            }
            else
            {
                var user = getLoggedUser();
                if (articleComment.author != user)
                    return View("AccessDenied");
                return View(articleComment);
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(ArticleComment articleComment)
        {
            articleCommentRepository.Update(articleComment);
            TempData["SuccessMessage"] = "Comment Updated Successfully!";
            var obj = articleCommentRepository.GetArticleComment(articleComment.articleCommentId);
            return RedirectToAction("Comments", "ArticleComment",obj.article.articleId);
        }

    }
}
