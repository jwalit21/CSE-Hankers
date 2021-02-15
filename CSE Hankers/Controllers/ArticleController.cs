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
    public class ArticleController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly AppDbContext context;

        public ArticleController(
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
        public IActionResult Index()
        {
            return View(context.Articles);
        }

        [Authorize]
        public IActionResult MyArticles()
        {
            var userid = signInManager.UserManager.GetUserId(User);
            var user = context.Users.Where(usr => usr.Id == userid).FirstOrDefault();

            var articles = context.Articles.Where(a => a.author == user);
            return View(articles);
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
                var userid = signInManager.UserManager.GetUserId(User);
                var user = context.Users.Where(usr => usr.Id == userid).FirstOrDefault();
                article.author = user;
                context.Articles.Add(article);
                context.SaveChanges();
                TempData["SuccessMessage"] = "Article Published Successfully!";
                return RedirectToAction("MyArticles");
            }
            return View(article);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Edit(int id)
        {
            Article article = context.Articles.FirstOrDefault(m => m.articleId == id);
            if (article == null)
            {
                TempData["ErrorMessage"] = "Article not found!";
                return RedirectToAction("Index", "Article");
            }
            else
            {
                var userid = signInManager.UserManager.GetUserId(User);
                var user = context.Users.Where(usr => usr.Id == userid).FirstOrDefault();
                if (article.author != user)
                    return RedirectToAction("AccessDenied", "Account");
                return View(article);
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(Article article)
        {
            var changedArticle = context.Articles.Attach(article);
            changedArticle.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();

            TempData["SuccessMessage"] = "Article Updated Successfully!";
            return RedirectToAction("Index", "Article");
        }

        [HttpGet]
        [Authorize]
        public IActionResult Delete(int id)
        {
            Article article = context.Articles.FirstOrDefault(m => m.articleId == id);

            if (article == null)
            {
                TempData["ErrorMessage"] = "Article not found!";
                return RedirectToAction("Index", "Article");
            }

            var userid = signInManager.UserManager.GetUserId(User);
            var user = context.Users.Where(usr => usr.Id == userid).FirstOrDefault();
            if (article.author != user)
                return RedirectToAction("AccessDenied", "Account");
            return View(article);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize]
        public IActionResult DeleteConfirmed(int id)
        {
            Article article = context.Articles.FirstOrDefault(m => m.articleId == id);
            if (article == null)
            {
                TempData["ErrorMessage"] = "Article not found!";
                return RedirectToAction("Index", "Article");
            }

            var articleLikes = context.ArticleLikes.Where(u => u.article == article);
            context.ArticleLikes.RemoveRange(articleLikes);
            context.SaveChanges();

            var articleComments = context.ArticleComments.Where(o => o.article == article);
            context.ArticleComments.RemoveRange(articleComments);
            context.SaveChanges();

            context.Articles.Remove(article);
            context.SaveChanges();


            TempData["SuccessMessage"] = "Article Deleted Successfully!";
            return RedirectToAction("Index", "Article");
        }


        [HttpGet]
        public IActionResult Details(int id)
        {
            Article article = context.Articles.FirstOrDefault(m => m.articleId == id);
            var userid = signInManager.UserManager.GetUserId(User);
            var user = context.Users.Where(usr => usr.Id == userid).FirstOrDefault();

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
                        return View("ArticleDetails", article);
                    }
                }
                return View(article);
            }
        }


        [HttpGet]
        [Authorize]
        public IActionResult Like(int id)
        {
            Article article = context.Articles.FirstOrDefault(m => m.articleId == id);
            var userid = signInManager.UserManager.GetUserId(User);
            var user = context.Users.Where(usr => usr.Id == userid).FirstOrDefault();

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
                    var alObject = new ArticleLikes()
                    {
                        article = article,
                        author = user,
                    };
                    context.ArticleLikes.Add(alObject);
                    context.SaveChanges();


                    article.likes = article.likes + 1;
                    var changedArticle = context.Articles.Attach(article);
                    changedArticle.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.SaveChanges();
                    TempData["SuccessMessage"] = "Article liked!";
                    return RedirectToAction("Details", "Article");
                }

                TempData["ErrorMessage"] = "Article already liked!";
                return RedirectToAction("Details", "Article");
            }
        }

    }
}
