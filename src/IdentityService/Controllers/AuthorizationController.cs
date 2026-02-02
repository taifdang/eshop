using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Controllers
{
    public class AuthorizationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Logout()
        {
            return View();
        }
    }
}
