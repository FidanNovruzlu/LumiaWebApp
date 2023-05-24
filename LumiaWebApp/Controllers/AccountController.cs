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
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;

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
                UserName=registerVM.Username
            };

            IdentityResult identityResult = await _userManager.CreateAsync(user,registerVM.Password);

            if (!identityResult.Succeeded)
            {
                foreach (IdentityError? error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    return View(registerVM);
                }
            }

            #region Add Role
            //IdentityResult result = await _userManager.AddToRoleAsync(user, "Admin");
            //if (!result.Succeeded)
            //{
            //    foreach(IdentityError? error in result.Errors)
            //    {
            //        ModelState.AddModelError("", error.Description);
            //        return View(registerVM);
            //    }
            //}
            #endregion

            return RedirectToAction(nameof(Login));

        }


        public async Task<IActionResult> Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM) 
        {
            if (!ModelState.IsValid)
            {
                return View(loginVM);
            }
            AppUser user = await _userManager.FindByNameAsync(loginVM.UserName);
            if(user == null)
            {
                ModelState.AddModelError("", "Invalid email or password");
                return View(loginVM);
            }
            Microsoft.AspNetCore.Identity.SignInResult signInResult =await _signInManager.PasswordSignInAsync(user,loginVM.Password, true, false);

            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("", "Invalid email or password");
                return View(loginVM);
            }
            return RedirectToAction("Index","Home");
        }


        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        #region Create Role
        //public async Task<IActionResult> CreateRole()
        //{
        //    IdentityRole role= new IdentityRole()
        //    {
        //        Name="Admin"
        //    };
        //    IdentityResult result= await _roleManager.CreateAsync(role);
        //    if (!result.Succeeded)  return NotFound();
        //    return Json("OK");
        //}
        #endregion
    }
}