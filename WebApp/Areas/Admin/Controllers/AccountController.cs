using Application.Services.Users;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.Areas.Admin.Models.Account;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("admin")]
    public class AccountController : Controller
    { 
        private readonly SignInManager<Users> _signInManager;

        public AccountController(SignInManager<Users> signInManager)
        {
            _signInManager = signInManager;
        }
        public IActionResult Login()
        {
            LoginVm model = new LoginVm();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginVm model)
        {
            if (ModelState.IsValid)
            {
                var identityResult = await _signInManager.PasswordSignInAsync(model.Email,model.Password, true, false);
                if (identityResult.Succeeded)
                {
                    return RedirectToAction("AddCountry", "Master");
                }
            }
            return View();
        }
     
        public async Task<IActionResult> LogoutAsync()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }


    }
}
