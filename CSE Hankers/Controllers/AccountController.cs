using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CSE_Hankers.Models;
using CSE_Hankers.Models.IRepositories;
using CSE_Hankers.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CSE_Hankers.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly AppDbContext context;
        private readonly IUserRepository userRepository;

        public AccountController(IHostingEnvironment hostingEnvironment,
            AppDbContext context,
            IUserRepository userRepository,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            this.userRepository = userRepository;
            this.hostingEnvironment = hostingEnvironment;
            this.roleManager = roleManager;
            this.context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterUser());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUser registerUser)
        {
            if (ModelState.IsValid)
            {
                //file upload
                string uniqueFileName = "";
                if (registerUser.photoPath != null)
                {
                    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "Images", "Profiles");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + registerUser.photoPath.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    registerUser.photoPath.CopyTo(new FileStream(filePath, FileMode.Create));
                }

                //copy data from registerviewmodel to identityuser
                //var maxUserId = userManager.Users.Max(usr => usr.userId);
                var user = new ApplicationUser
                {
                    userId = 0,
                    name = registerUser.name,
                    UserName = registerUser.email,
                    Email = registerUser.email,
                    organization = registerUser.organization,
                    mobile = registerUser.mobile,
                    photoPath = uniqueFileName
                };

                var result = await userManager.CreateAsync(user, registerUser.password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "User");

                    TempData["SuccessMessage"] = "User Registered successfully!";
                    return RedirectToAction("Login", "Account");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            
            return View(registerUser);
        }

        [HttpGet]
        public IActionResult Login()
        {
            ViewBag.role = "User";
            return View();
        }

        [Route("Account/Login/{role}")]
        [HttpGet]
        public IActionResult Login(string role)
        {
            //Role is for identifying the Admin login
            if (role == "Admin")
                ViewBag.role = role;
            else
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.email, model.password, model.rememberMe, false);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return LocalRedirect(returnUrl);
                    }
                    else
                    {
                        TempData["SuccessMessage"] = "Welcome " + model.email + "!";

                        //If logged user is Admin then
                        if (model.role == "Admin")
                        {
                            return RedirectToAction("Dashboard", "Admin");
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
                await signInManager.SignOutAsync();
            }
            return View(model);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Profile(string? username)
        {
            ApplicationUser user = await userManager.GetUserAsync(User);
            bool isOwnProfile = true; 
            if (username!= null)
            {
                ApplicationUser applicationUser = userManager.Users.Where(u => u.UserName == username).FirstOrDefault();
                if (applicationUser == null)
                    return RedirectToAction("404", "Account");
                
                isOwnProfile = false;
                if (applicationUser.Id == user.Id)
                    isOwnProfile = true;
                user = applicationUser;
            }
            ViewUser viewUser = new ViewUser()
            {
                id = user.userId,
                name = user.name,
                email = user.Email,
                mobile = user.mobile,
                organization = user.organization,
                photoPath = user.photoPath
            };

            ViewBag.isOwnProfile = isOwnProfile;
            return View(viewUser);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> UpdateProfile()
        {
            ApplicationUser user = await userManager.GetUserAsync(User);
            UpdateUser updateUser = new UpdateUser()
            {
                id = user.userId,
                email = user.Email,
                name = user.name,
                mobile = user.mobile,
                organization = user.organization,
                existingPhotoPath = user.photoPath,
            };
            return View(updateUser);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateProfile(UpdateUser updateUser)
        {
            ApplicationUser user = await userManager.FindByEmailAsync(updateUser.email);
            if (user == null)
                return RedirectToAction("Profile");

            if(ModelState.IsValid)
            {
                user.mobile = updateUser.mobile;
                user.name = updateUser.name;
                user.organization = updateUser.organization;
                if(updateUser.photoPath != null)
                { 
                    if(updateUser.existingPhotoPath != null)
                    {
                        string filePath = Path.Combine(hostingEnvironment.WebRootPath, "Images", "Profiles", updateUser.existingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }
                    string uniqueFileName = "";
                    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "Images", "Profiles");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + updateUser.photoPath.FileName;
                    string newfilePath = Path.Combine(uploadsFolder, uniqueFileName);
                    updateUser.photoPath.CopyTo(new FileStream(newfilePath, FileMode.Create));

                    user.photoPath = uniqueFileName;
                }

                userRepository.Update(user);
                return RedirectToAction("Profile");
            }

            return View(updateUser);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            TempData["SuccessMessage"] = "Logged out Successfully!";
            return RedirectToAction("Login", "Account");
        }
    }
}
