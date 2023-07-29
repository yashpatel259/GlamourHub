using GlamourHub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GlamourHub.Controllers
{
    public class OrderController : Controller
    {
        private readonly GlamourHubContext _dbContext;
        public OrderController(GlamourHubContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult OrdersList()
        {
            // Check if session data exists
            if (!ValidateRole())
            {
                // Redirect to the login page if session data is missing
                return RedirectToAction("Index", "Login");
            }

            // Fetch all orders from the database with relevant information, including user details
            var orders = _dbContext.Order
                .Include(o => o.order_items) // Include order items for counting
                .OrderByDescending(o => o.OrderDate) // Sort by OrderDate in descending order
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
            return HttpContext.Session.GetString("Role") == "Admin" || HttpContext.Session.GetString("Role") == "Seller" ? true : false;
        }
    }
}
