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
                    string encodedPassword = EncodePasswordToBase64(user.Password);
                    user.Password = encodedPassword;

                    using (var context = new GlamourHubContext())
                    {
                        context.Users.Add(user);
                        context.SaveChanges();
                    }
                    return View();
                }

                // Validation errors exist, return the view with errors
                return View(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View("Error");
            }
        }


        //this function Convert to Encord your Password
        public static string EncodePasswordToBase64(string password)
        {
            try
            {
                byte[] encData_byte = new byte[password.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }
       
    }
}
