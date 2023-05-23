using LumiaWebApp.DAL;
using LumiaWebApp.Models;
using LumiaWebApp.ViewModels;
using LumiaWebApp.ViewModels.AccountVM;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LumiaWebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Register()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View(registerVM);
            }

            AppUser user = new AppUser()
            {
                Name = registerVM.Name,
                Email = registerVM.Email,
                Surname = registerVM.Surname,

            };

            IdentityResult identityResult = await _userManager.CreateAsync(user,registerVM.Password);

            if (!identityResult.Succeeded)
            {
                foreach (var error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    return View(registerVM);
                }
            }

           
            return RedirectToAction(nameof(Login));

        }
        public async Task<IActionResult> Login()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM) 
        {
            if (!ModelState.IsValid)
            {
                return View(loginVM);
            }
            AppUser user = await _userManager.FindByEmailAsync(loginVM.Email);
            if(user == null)
            {
                ModelState.AddModelError("", "Invalid email or password");
                return View(loginVM);
            }
           Microsoft.AspNetCore.Identity.SignInResult signInResult =await _signInManager.PasswordSignInAsync(user, user.Email, true, false);

            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("", "Invalid email or password");
                return View(loginVM);
            }
            return RedirectToAction("Index","Home");
        }
    }
}