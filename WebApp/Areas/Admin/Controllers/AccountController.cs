using Microsoft.AspNetCore.Mvc;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("admin")]
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}
