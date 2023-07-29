using GlamourHub.DataAccess;
using GlamourHub.Models;
using GlamourHub.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace GlamourHub.Controllers
{
    public class CheckOutController : Controller
    {

        private readonly GlamourHubContext _dbContext;
        public CheckOutController(GlamourHubContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: /CheckOut/Index
        public IActionResult Index()
        {
            // Check if session data exists
            if (!ValidateRole())
            {
                // Redirect to the login page if session data is missing
                return RedirectToAction("Index", "Login");
            }

            var cart = GetCartItems();

            // Check if the cart has at least one item
            if (cart.Count == 0)
            {
                // Redirect to the cart page with a message
                TempData["Message"] = "Please add products to the cart before checkout.";
                return RedirectToAction("Index", "Cart");
            }

            var checkoutViewModel = new CheckoutViewModel
            {
                CartItems = cart,
                //ShippingAddress = new Address() // Create a new instance of the Address model for the shipping address
            };

            // Calculate the order summary values and update the model
            CalculateOrderSummary(checkoutViewModel);

            return View(checkoutViewModel);
        }

        [HttpPost]
        public IActionResult CompleteOrder(CheckoutViewModel model, string CartItemsJson)
        {
            // Get the UserId using the GetUserId() method
            int userId = GetUserId();

            // Check if the cart has at least one item
            model.CartItems = JsonConvert.DeserializeObject<List<Cart>>(CartItemsJson);

            try
            {
                // Check if the model state is valid
                if (ModelState.IsValid)
                {
                    // Calculate the order summary values
                    CalculateOrderSummary(model);

                    // Save the order to the database
                    var order = new Order
                    {
                        UserId = userId,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Street = model.Street,
                        City = model.City,
                        State = model.State,
                        PostalCode = model.PostalCode,
                        Country = model.Country,
                        Phone = model.Phone,
                        TotalAmount = model.TotalAmount,
                        TaxAmount = model.TaxAmount,
                        DeliveryAmount = model.DeliveryAmount,
                        GrandTotal = model.GrandTotal,
                        IsFreeShipping = model.IsFreeShipping,
                        OrderDate = DateTime.Now
                    };

                    _dbContext.Order.Add(order);
                    _dbContext.SaveChanges();

                    // Save the order items to the database
                    foreach (var cartItem in model.CartItems)
                    {
                        // Retrieve the product entity using the ProductId
                        var product = _dbContext.Products.FirstOrDefault(p => p.Id == cartItem.ProductId);

                        if (product != null)
                        {
                            var orderItem = new order_items
                            {
                                OrderId = order.Id,
                                ProductId = (int)cartItem.ProductId,
                                Quantity = cartItem.Quantity,
                                Price = product.Price // Access the product's Price property
                            };

                            _dbContext.order_items.Add(orderItem);
                        }
                    }
                    // Clear the cart
                    ClearCart();

                    _dbContext.SaveChanges();

                    // Redirect to a success page or perform any additional actions
                    return RedirectToAction("ThankYou", "CheckOut");
                }
            }
            catch (Exception)
            {
                // Handle any exceptions that might occur during the database operations
            }

            var cart = GetCartItems();
            if (cart.Count == 0)
            {
                TempData["Message"] = "Please add products to the cart before checkout.";
                return RedirectToAction("Index", "Cart");
            }

            model.CartItems = cart;
            // If the form data is invalid, return the view with validation errors
            // Calculate the order summary values and update the model
            CalculateOrderSummary(model);

            return View("Index", model);
        }


        public IActionResult ThankYou()
        {
            // Check if session data exists
            if (!ValidateRole())
            {
                // Redirect to login page if session data is missing
                return RedirectToAction("Index", "Login");
            }
            
            return View();
        }

        private void CalculateOrderSummary(CheckoutViewModel model)
        {
            var cart = GetCartItems();
            decimal totalAmount = cart.Sum(item => item.Product.Price * item.Quantity);
            decimal taxRate = 0.13m; // 13% tax rate
            decimal deliveryRate = 0.05m; // 5% delivery rate
            decimal taxAmount = totalAmount * taxRate;
            decimal deliveryAmount = totalAmount < 50 ? totalAmount * deliveryRate : 0;
            decimal grandTotal = totalAmount + taxAmount + deliveryAmount;
            bool isFreeShipping = totalAmount > 50 && deliveryAmount <= 0;

            model.TotalAmount = totalAmount;
            model.TaxAmount = taxAmount;
            model.DeliveryAmount = deliveryAmount;
            model.GrandTotal = grandTotal;
            model.IsFreeShipping = isFreeShipping;
        }


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