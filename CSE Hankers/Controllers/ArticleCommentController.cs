using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSE_Hankers.Models;
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

        public ArticleCommentController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            AppDbContext context)
        {
            this.context = context;
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Comments(int id)
        {
            Article article = context.Articles.FirstOrDefault(m => m.articleId == id);
            if (article == null)
            {
                TempData["ErrorMessage"] = "Article not found!";
                return View("404");
            }

            var userid = signInManager.UserManager.GetUserId(User);
            var user = context.Users.Where(usr => usr.Id == userid).FirstOrDefault();
            var comments = context.ArticleComments.Where(l => l.author==user).Where(p => p.article.articleId==id);
            ViewBag.id = id;
            ViewBag.user = user;
            return View(comments);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create(int id)
        {
            Article article = context.Articles.FirstOrDefault(m => m.articleId == id);
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
                int articleId = articleComment.likes;
                articleComment.likes = 0;
                var userid = signInManager.UserManager.GetUserId(User);
                var user = context.Users.Where(usr => usr.Id == userid).FirstOrDefault();
                articleComment.author = user;

                
                Article article = context.Articles.FirstOrDefault(m => m.articleId == articleId);
                if (article == null)
                {
                    TempData["ErrorMessage"] = "Article not found!";
                    return View("404");
                }
 
                articleComment.article = article;
                context.ArticleComments.Add(articleComment);
                context.SaveChanges();
                TempData["SuccessMessage"] = "comment added Successfully!";
                return RedirectToAction("Comments","ArticleComment",articleId);
            }
            return View(articleComment);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Edit(int id)
        {
            ArticleComment articleComment = context.ArticleComments.FirstOrDefault(m => m.articleCommentId == id);
            if (articleComment == null)
            {
                TempData["ErrorMessage"] = "Comment not found!";
                return View("404");
            }
            else
            {
                var userid = signInManager.UserManager.GetUserId(User);
                var user = context.Users.Where(usr => usr.Id == userid).FirstOrDefault();
                if (articleComment.author != user)
                    return View("AccessDenied");
                return View(articleComment);
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(ArticleComment articleComment)
        {
            
            var changedArticleComment = context.ArticleComments.Attach(articleComment);
            changedArticleComment.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();

            TempData["SuccessMessage"] = "Comment Updated Successfully!";
            var obj = context.ArticleComments.FirstOrDefault(o => o.articleCommentId == articleComment.articleCommentId);
            return RedirectToAction("Comments", "ArticleComment",obj.article.articleId);
        }

    }
}
