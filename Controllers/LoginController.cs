using Microsoft.AspNetCore.Mvc;
using GlamourHub.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.Design;
using Microsoft.AspNetCore.Identity;
using System.Net.Mail;
using System.Net;

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
                        if (!user.IsEmailVerified)
                        {
                            ViewData["ErrorMessage"] = "Please verify your email address before logging in.";
                            ViewData["ShowResendVerification"] = true; 
                            return View(model);
                        }

                        string storedPassword = user.Password;
                        string PasswordEn = EncodePasswordToBase64(PasswordE);
                        if (storedPassword == PasswordEn)
                        {
                            // Start a new session
                            HttpContext.Session.Clear();
                            HttpContext.Session.SetString("Username", user.Username);
                            HttpContext.Session.SetString("Role", user.Role);
                            HttpContext.Session.SetString("Firstname", user.Firstname);

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

        public IActionResult ResendVerificationEmail()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ResendVerificationEmail(string email)
        {
            using (var context = new GlamourHubContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Email == email);

                if (user != null)
                {
                    // Generate a new verification token
                    string verificationToken = Guid.NewGuid().ToString();

                    // Update the user's verification token in the database
                    user.VerificationToken = verificationToken;
                    context.SaveChanges();

                    // Send the verification email
                    // ... (remaining code for sending the email)

                    // Resend the verification email
                    var client = new SmtpClient("smtp.gmail.com", 587)
                    {
                        Credentials = new NetworkCredential("glamourhub04@gmail.com", "fqsurzmnlynhkknd"),
                        EnableSsl = true
                    };

                    string verificationUrl = $"{Request.Scheme}://{Request.Host}/Register/ConfirmEmail?token={verificationToken}";

                    string emailBody = $"Please verify your email by clicking on this link: <a href='{verificationUrl}'>Verify Email</a>";

                    MailMessage mm = new MailMessage();
                    mm.To.Add(new MailAddress(user.Email));
                    mm.From = new MailAddress("glamourhub04@gmail.com");
                    mm.Subject = "Confirm Email";
                    mm.Body = emailBody;
                    mm.IsBodyHtml = true;

                    client.Send(mm);

                    ViewData["SuccessMessage"] = "A new verification email has been sent to your email address. Please check your inbox and click on the link to complete the registration.";
                }
                else
                {
                    ViewData["ErrorMessage"] = "Email address not found. Please check your email or register again.";
                }
            }

            return View();
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
