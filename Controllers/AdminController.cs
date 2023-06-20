using Microsoft.AspNetCore.Mvc;

namespace GlamourHub.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
