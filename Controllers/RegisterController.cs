using GlamourHub.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;

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
                        // Generate a verification token
                        string verificationToken = Guid.NewGuid().ToString();

                        // Save the token and other registration data to the database
                        user.VerificationToken = verificationToken;
                        user.CreatedAt = DateTime.Now;

                        // Hash the password before saving it to the database
                        user.Password = EncodePasswordToBase64(user.Password);

                        context.Users.Add(user);
                        context.SaveChanges();

                        // Send the verification email
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
                    }

                    ViewData["SuccessMessage"] = "A verification email has been sent to your email address. Please check your inbox and click on the link to complete the registration.";
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

        public IActionResult ConfirmEmail(string token)
        {
            using (var context = new GlamourHubContext())
            {
                // Find the user with the given verification token
                var user = context.Users.FirstOrDefault(u => u.VerificationToken == token);

                if (user != null)
                {
                    // Mark the user's email as verified
                    user.IsEmailVerified = true;
                    context.SaveChanges();

                    TempData["SuccessMessage"] = "Your email address has been verified successfully. You can now log in with your credentials.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Invalid verification token. Please try again or contact support.";
                }

                return  View();
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
