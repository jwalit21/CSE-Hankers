using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CSE_Hankers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using CSE_Hankers.Models.IRepositories;

namespace CSE_Hankers.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IArticleRepository articleRepository;

        public HomeController(AppDbContext context, 
            ILogger<HomeController> logger,
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager,
            IArticleRepository articleRepository)
        {
            this.articleRepository = articleRepository;
            this.context = context;
            this.signInManager = signInManager;
            this.userManager = userManager;
            _logger = logger;
        }

        public ApplicationUser getLoggedUser()
        {
            var userid = signInManager.UserManager.GetUserId(User);
            var user = context.Users.Where(usr => usr.Id == userid).FirstOrDefault();
            return user;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            Dictionary<ApplicationUser, int> userLikes = new Dictionary<ApplicationUser, int>();
            IList<Article> articlesTotal = context.Articles.ToList();
            foreach (var item in articlesTotal)
            {
                int articleLike = context.ArticleLikes.Where(al => al.article.articleId == item.articleId).Count();
                var articleUser = await userManager.FindByIdAsync(item.authorId);
                if(!userLikes.ContainsKey(articleUser))
                {
                    userLikes.Add(articleUser, articleLike);
                }
            }

            int userLikesCount = userLikes.Count() > 3 ? 3 : userLikes.Count();

            ApplicationUser user = getLoggedUser();
            int articles = articleRepository.GetAllArticles().Where(a => a.authorId == user.Id).Count();
            ViewBag.articles = articles;
            ViewBag.userLikes = userLikes;
            ViewBag.userLikesCount = userLikesCount;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult ContactUs()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
