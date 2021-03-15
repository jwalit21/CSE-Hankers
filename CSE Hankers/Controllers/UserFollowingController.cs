using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSE_Hankers.Models;
using CSE_Hankers.Models.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CSE_Hankers.Controllers
{
    public class UserFollowingController : Controller
    {

        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly AppDbContext context;
        private readonly IUserRepository userRepository;

        public UserFollowingController(AppDbContext context,
            IUserRepository userRepository,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            this.userRepository = userRepository;
            this.roleManager = roleManager;
            this.context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public ApplicationUser getLoggedUser()
        {
            var userid = signInManager.UserManager.GetUserId(User);
            var user = context.Users.Where(usr => usr.Id == userid).FirstOrDefault();
            return user;
        }

        [Authorize]
        public ActionResult Users()
        {
            ApplicationUser user = getLoggedUser();
            IEnumerable<UserFollowing> followings = context.UserFollowings.Where(uf => uf.user == user).ToList();
            IList<ApplicationUser> applicationUsers = userManager.Users.ToList();
            var AllUnfollowedUsers = Enumerable.Empty<ApplicationUser>();
            var TotalUsers = AllUnfollowedUsers.ToList();
            foreach (var item in followings)
            {
                applicationUsers.Remove(applicationUsers.Where(au => au.Id != item.user.Id).FirstOrDefault());
            }
            applicationUsers.Remove(user);
            return View("Users",applicationUsers);
        }


        [Authorize]
        public async Task<ActionResult> Followed()
        {
            ApplicationUser user = getLoggedUser();
            IEnumerable<UserFollowing> followings = context.UserFollowings.Where(uf => uf.user == user).ToList();
            foreach (var item in followings)            
            {
                item.follower = await userManager.FindByIdAsync(item.followerId);
            }
            return View("FollowedUsers",followings);
        }

        [Authorize]
        public ActionResult Follow(string username)
        {
            ApplicationUser applicationUser = userManager.Users.Where(u => u.UserName == username).FirstOrDefault();
            if (applicationUser == null)
                return RedirectToAction("404", "Account");

            ApplicationUser user = getLoggedUser();

            if(context.UserFollowings.Where(uf => (uf.user==user && uf.follower==applicationUser)).FirstOrDefault() != null)
            {
                TempData["ErrorMessage"] = "User already Followed!";
                return RedirectToAction("Users", "UserFollowing");
            }   
            
            UserFollowing userFollowing = new UserFollowing()
            {
                user = user,
                follower = applicationUser,
            };
            userRepository.Follow(userFollowing);
            TempData["SuccessMessage"] = "User Followed!";
            return RedirectToAction("Index","Article");
        }

        public ActionResult Unfollow(string username)
        {
            ApplicationUser applicationUser = userManager.Users.Where(u => u.UserName == username).FirstOrDefault();
            if (applicationUser == null)
                return RedirectToAction("404", "Account");

            ApplicationUser user = getLoggedUser();

            if (context.UserFollowings.Where(uf => (uf.user == user && uf.follower == applicationUser)).FirstOrDefault() == null)
            {
                TempData["ErrorMessage"] = "User not Followed!";
                return RedirectToAction("Users", "UserFollowing");
            }

            UserFollowing userFollowing = context.UserFollowings.Where(uf => (uf.user.Id == user.Id && uf.follower.Id == applicationUser.Id)).FirstOrDefault();
            userRepository.Unfollow(userFollowing);
            TempData["SuccessMessage"] = "User Unfollowed!";
            return RedirectToAction("Index", "Article");
        }
    }
}
