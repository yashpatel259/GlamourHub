using Microsoft.AspNetCore.Mvc;

namespace GlamourHub.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
