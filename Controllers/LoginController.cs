using Microsoft.AspNetCore.Mvc;
using GlamourHub.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.Design;

namespace GlamourHub.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(Login model)
        {
            if (ModelState.IsValid)
            {
                using (var context = new GlamourHubContext())
                {
                    var stores = from store in context.Users select store.Username;

                    var UsernameE = model.Username;
                    var PasswordE = model.Password;
                    

                    var user = context.Users.FirstOrDefault(store => store.Username == UsernameE);

                    if (user != null)
                    {
                        string storedPassword = user.Password;
                        string PasswordEn = EncodePasswordToBase64(PasswordE);
                        if (storedPassword == PasswordEn)
                        {
                            // Start a new session
                            HttpContext.Session.Clear();
                            HttpContext.Session.SetString("Username", user.Username);

                            // Set session expiration time to 20 minutes
                            HttpContext.Session.SetString("ExpirationTime", DateTime.Now.AddMinutes(20).ToString());

                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ViewData["ErrorMessage"] = "Invalid password.";
                        }
                    }
                    else
                    {
                        ViewData["ErrorMessage"] = "Invalid username.";
                    }
                }
            }
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Clear the session
            return RedirectToAction("Index", "Home"); // Redirect to the Home page 
        }


        //this function Convert to Decord Password

        //public string DecodeFrom64(string encodedData)
        //{
        //    System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
        //    System.Text.Decoder utf8Decode = encoder.GetDecoder();
        //    byte[] todecode_byte = Convert.FromBase64String(encodedData);
        //    int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
        //    char[] decoded_char = new char[charCount];
        //    utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
        //    string result = new String(decoded_char);
        //    return result;
        //}

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
