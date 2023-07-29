using Microsoft.AspNetCore.Mvc;
using GlamourHub.Models;
using System.Collections.Generic;
using GlamourHub.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace GlamourHub.Controllers
{
    
    public class AdminController : Controller
    {
        private readonly GlamourHubContext _dbContext;

        public AdminController(GlamourHubContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            // Check if session data exists
            if (!ValidateRole())
            {
                // Redirect to login page if session data is missing
                return RedirectToAction("Index", "Login");
            }
            return View();
        }

        [HttpGet]
        public IActionResult UserList()
        {
            // Check if session data exists
            if (!ValidateRole())
            {
                // Redirect to login page if session data is missing
                return RedirectToAction("Index", "Login");
            }
            IEnumerable<User> user = _dbContext.Users;

            return View(user);
        }


        [HttpGet]
        public IActionResult AddUser()
        {
            // Check if session data exists
            if (!ValidateRole())
            {
                // Redirect to login page if session data is missing
                return RedirectToAction("Index", "Login");
            }
            return View();
        }

        [HttpPost]
        public ActionResult AddUser(User user)
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
                    TempData["SuccessMessage"] = "User added successfully!";
                    return View(user); // Redirect to a success page or another appropriate action
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

        [HttpGet]
        public IActionResult EditUser(int id)
        {
            // Check if session data exists
            if (!ValidateRole())
            {
                // Redirect to login page if session data is missing
                return RedirectToAction("Index", "Login");
            }
            User existingUser = _dbContext.Users.Find(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            return View(existingUser);
        }

        [HttpPost]
        public IActionResult EditUser(User user)
        {
            if (ModelState.IsValid)
            {
                User existingUser = _dbContext.Users.Find(user.Id);
                if (existingUser == null)
                {
                    return NotFound();
                }

                // Check if the updated username already exists (excluding the current user)
                bool isUsernameTaken = _dbContext.Users.Any(u => u.Username == user.Username && u.Id != user.Id);
                if (isUsernameTaken)
                {
                    ModelState.AddModelError("Username", "Username is already taken.");
                    return View(user);
                }

                // Update the properties of the existing user
                existingUser.Firstname = user.Firstname;
                existingUser.Lastname = user.Lastname;
                existingUser.Role = user.Role;
                existingUser.Username = user.Username;

                user.Password = existingUser.Password;
                _dbContext.SaveChanges();
                return RedirectToAction("UserList");
            }

            // Retrieve and examine ModelState errors
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var error in errors)
            {
                // Log or display the error messages as needed
                Console.WriteLine(error.ErrorMessage);
            }

            return View(user);
        }

        [HttpGet]
        public IActionResult DeleteUser(int id)
        {
            // Check if session data exists
            if (!ValidateRole())
            {
                // Redirect to login page if session data is missing
                return RedirectToAction("Index", "Login");
            }
            User existingUser = _dbContext.Users.Find(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            return View(existingUser);
        }

        [HttpPost]
        public IActionResult ConfirmDeleteUser(int id)
        {
            User existingUser = _dbContext.Users.Find(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            _dbContext.Users.Remove(existingUser);
            _dbContext.SaveChanges();

            return RedirectToAction("UserList");
        }

        [HttpGet]
        public IActionResult OrdersList()
        {
            // Check if session data exists
            //if (!ValidateRole())
            //{
            //    // Redirect to the login page if session data is missing
            //    return RedirectToAction("Index", "Login");
            //}

            // Fetch all orders from the database with relevant information, including user details
            var orders = _dbContext.Order
                .Include(o => o.order_items) // Include order items for counting
                .ToList();

            // Create a list of OrderSummaryViewModel
            var orderSummaries = orders.Select(o => new OrderSummaryViewModel
            {
                OrderId = o.Id,
                CustomerName = $"{o.FirstName} {o.LastName}",
                ItemCount = o.order_items.Count, // Count the number of order items
                OrderDate = o.OrderDate,
                TotalBill = o.GrandTotal
            }).ToList();

            return View(orderSummaries);
        }


        [HttpGet]
        public IActionResult OrderDetails(int id)
        {
            // Fetch the order with the given id, including the related user and order items
            Order order = _dbContext.Order
                .Include(o => o.order_items)
                .ThenInclude(item => item.Product)
                .FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            // Now we can access user-related properties using the UserId
            User user = _dbContext.Users.FirstOrDefault(u => u.Id == order.UserId);
            if (user != null)
            {
                order.FirstName = user.Firstname;
                order.LastName = user.Lastname;
            }

            return View(order);
        }


        public bool ValidateRole()
        {
            return HttpContext.Session.GetString("Role") == "Admin" ? true : false;
        }

    }
}
