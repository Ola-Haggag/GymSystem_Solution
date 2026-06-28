using GymSystem.BLL.ViewModels.ApplicationViewModels;
using GymSystem.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model , CancellationToken ct)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await userManager.FindByEmailAsync(model.Email);

            if(user is null || string.IsNullOrEmpty(user.UserName))
            {
                ModelState.AddModelError(string.Empty, "Invalid Email or Password");
                return View(model);
            }
            var Result = await signInManager.PasswordSignInAsync(user.UserName, model.Password,model.RememberMe, true);
            if(Result.Succeeded)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            if (Result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "This Account is LockedOut");
            }
            if (Result.IsNotAllowed)
            {
                ModelState.AddModelError(string.Empty, "Un Authorized");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid Email or Password");
            }
            return View(model);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
