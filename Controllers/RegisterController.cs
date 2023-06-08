using GlamourHub.Models;
using Microsoft.AspNetCore.Mvc;

namespace GlamourHub.Controllers
{
    public class RegisterController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (var context = new GlamourHubContext())
                    {
                        context.Users.Add(user);
                        context.SaveChanges();
                    }

                    // Redirect to a success page or perform any other desired action
                    return RedirectToAction("Index", "Home");
                }

                // If the form data is not valid, return the view with validation errors
                return View(user);

            }
            catch (Exception ex)
            {
                return null;
            }
            
        }
    }
}
