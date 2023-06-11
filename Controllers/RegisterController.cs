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
                        // Check if email already exists
                        bool isEmailRegistered = context.Users.Any(u => u.Email == user.Email);
                        if (isEmailRegistered)
                        {
                            ModelState.AddModelError("Email", "You have already registered with this email.");
                            return View(user);
                        }

                        // Check if username already exists
                        bool isUsernameTaken = context.Users.Any(u => u.Username == user.Username);
                        if (isUsernameTaken)
                        {
                            ModelState.AddModelError("Username", "Username is already taken.");
                            return View(user);
                        }                       

                        // Encode password
                        string encodedPassword = EncodePasswordToBase64(user.Password);
                        user.Password = encodedPassword;

                        // Save the user to the database
                        user.CreatedAt = DateTime.Now;
                        context.Users.Add(user);
                        context.SaveChanges();
                    }
                    TempData["SuccessMessage"] = "You have registered successfully!";
                    return View(user);
                }

                return View(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View("Error");
            }
        }

        //this function Convert to Encord Password
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
