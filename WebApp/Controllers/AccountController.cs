using Application.Services.Users;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models.Account;

namespace WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<Users> _signInManager;
        private readonly IUserServices _userServices;

        public AccountController(IUserServices userServices, SignInManager<Users> signInManager)
        {
            _userServices = userServices;
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
                var identityResult = await _signInManager.PasswordSignInAsync(model.Email, model.Password, true, false);
                if (identityResult.Succeeded)
                {
                    return RedirectToAction("DashBoard", "Home", new { @area = "admin" });
                    //if(User.IsInRole("SuperAdmin"))
                    //    return RedirectToAction("DashBoard", "Home", new { @area="admin"});
                    //else
                    //    return RedirectToAction("DashBoard", "Account");
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogoutAsync()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
        public IActionResult DashBoard()
        {
            return View();
        }

    }
}