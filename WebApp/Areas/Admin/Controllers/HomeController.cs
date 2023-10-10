using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult DashBoard()
        {
            return View();
        }
    }
}
