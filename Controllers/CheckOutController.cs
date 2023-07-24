using GlamourHub.DataAccess;
using GlamourHub.Models;
using GlamourHub.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GlamourHub.Controllers
{
    public class CheckOutController : Controller
    {

        private readonly ApplicationDbContext _dbContext;
        public CheckOutController(ApplicationDbContext dbContext) {
            _dbContext = dbContext;
        }

        // GET: /CheckOut/Index
        public IActionResult Index()
        {
            // Check if session data exists
            if (!ValidateRole())
            {
                // Redirect to login page if session data is missing
                return RedirectToAction("Index", "Login");
            }

            var cart = GetCartItems();

            // Check if cart has at least one item
            if (cart.Count == 0)
            {
                // Redirect to cart page with a message
                TempData["Message"] = "Please add products to the cart before checkout.";
                return RedirectToAction("Index", "Cart");
            }

            var userId = GetUserId();

            var checkoutViewModel = new CheckoutViewModel
            {
                CartItems = cart,
                BillingAddress = new Address(), // Create a new instance of the Address model for billing address
                ShippingAddress = new Address() // Create a new instance of the Address model for shipping address
            };

            return View(checkoutViewModel);
        }



        public IActionResult ThankYou()
        {
            // Check if session data exists
            if (!ValidateRole())
            {
                // Redirect to login page if session data is missing
                return RedirectToAction("Index", "Login");
            }
            // Clear the cart
            ClearCart();
            return View();
        }

        [HttpPost]
        public IActionResult CompleteOrder(CheckoutViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Access the billing and shipping address from the model
                Address billingAddress = model.BillingAddress;
                Address shippingAddress = model.ShippingAddress;

                // Save the addresses to the database using your data access logic
                _dbContext.Address.Add(billingAddress);
                _dbContext.Address.Add(shippingAddress);
                _dbContext.SaveChanges();

                // Redirect to a success page or perform any additional actions
                return RedirectToAction("ThankYou", "CheckOut");
            }

            // If the form data is invalid, return the view with validation errors
            return View("Index", model);
        }



        //[HttpPost]
        //public IActionResult ThankYou(OrderViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Create and populate the Address object for billing and shipping address
        //        var billingAddress = new Address
        //        {
        //            BillingFirstName = model.FirstName,
        //            BillingLastName = model.LastName,
        //            BillingStreet = model.Address,
        //            BillingCity = model.City,
        //            BillingState = model.State,
        //            BillingPostalCode = model.PostalCode
        //        };

        //        var shippingAddress = model.SameAsBilling ? billingAddress : new Address
        //        {
        //            ShippingFirstName = model.ShippingFirstName,
        //            ShippingLastName = model.ShippingLastName,
        //            ShippingStreet = model.ShippingAddress,
        //            ShippingCity = model.ShippingCity,
        //            ShippingState = model.ShippingState,
        //            ShippingPostalCode = model.ShippingPostalCode
        //        };

        //        // Save the address objects to the database
        //        _dbContext.Addresses.Add(billingAddress);
        //        if (!model.SameAsBilling)
        //            _dbContext.Addresses.Add(shippingAddress);
        //        _dbContext.SaveChanges();

        //        // Create the Order object
        //        var order = new Order
        //        {
        //            UserId = GetUserId(),
        //            OrderDate = DateTime.Now,
        //            TotalAmount = model.TotalAmount,
        //            DeliveryCharge = model.DeliveryAmount,
        //            BillingAddressId = billingAddress.Id,
        //            ShippingAddressId = model.SameAsBilling ? billingAddress.Id : shippingAddress.Id,
        //            // Other order properties
        //        };

        //        // Save the order to the database
        //        _dbContext.Orders.Add(order);
        //        _dbContext.SaveChanges();

        //        // Clear the cart or perform any other necessary operations

        //        // Redirect to a thank you page or show a success message
        //        return RedirectToAction("ThankYou");
        //    }

        //    // If the form data is invalid, redisplay the checkout page with validation errors
        //    return View("Checkout", model);
        //}


        private void ClearCart()
        {
            var userId = GetUserId();
            var cart = _dbContext.Cart.Where(cart => cart.UserId == userId);
            _dbContext.Cart.RemoveRange(cart);
            _dbContext.SaveChanges();
        }
        public bool ValidateRole()
        {
            return HttpContext.Session.GetString("Role") == "Customer" || HttpContext.Session.GetString("Role") == "Admin" || HttpContext.Session.GetString("Role") == "Seller" ? true : false;
        }

        private List<Cart> GetCartItems()
        {
            var userId = GetUserId();
            return _dbContext.Cart.Include(cart => cart.Product).Where(cart => cart.UserId == userId).ToList();
        }

        private int GetUserId()
        {
            var username = HttpContext.Session.GetString("Username");

            var user = _dbContext.Users.FirstOrDefault(u => u.Username == username);

            if (user != null)
            {
                return user.Id;
            }

            throw new Exception("User not found.");
        }
    }
}
