using GlamourHub.DataAccess;
using GlamourHub.Models;
using GlamourHub.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Stripe;
using Stripe.Checkout;

namespace GlamourHub.Controllers
{
    public class CheckOutController : Controller
    {

        private readonly GlamourHubContext _dbContext;
        private readonly string _stripeSecretKey = "sk_test_51NahmtE7u4zmWjouuyhi50nyPPbN1ywQeWGQO7AWsNRXF5sDTwiLsYl0BmXPdOUbBCbgtVzxGqHNFVU9TIRzvNgF00d5IOi7Cb";

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


                    // Create a new order instance with IsPaymentSuccess set to false
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
                        OrderDate = DateTime.Now,
                        IsPaymentSuccess = false // Set IsPaymentSuccess to false initially
                    };

                    _dbContext.Order.Add(order);
                    _dbContext.SaveChanges();

                    var lastRecordId = _dbContext.Order.Where(order => order.UserId == userId).Max(x => x.Id);

                    // Save the order ID in session
                    HttpContext.Session.SetInt32("OrderId", lastRecordId);

                    // Create a list to store the line items for the session
                    var lineItems = new List<SessionLineItemOptions>();

                    // Add each product in the cart to the line items
                    foreach (var cartItem in model.CartItems)
                    {
                        // Retrieve the product entity using the ProductId
                        var product = _dbContext.Products.FirstOrDefault(p => p.Id == cartItem.ProductId);

                        if (product != null)
                        {
                            // Add the product details to the line items
                            lineItems.Add(new SessionLineItemOptions
                            {
                                PriceData = new SessionLineItemPriceDataOptions
                                {
                                    UnitAmount = (long)(product.Price * 100), // Amount in cents
                                    Currency = "cad",
                                    ProductData = new SessionLineItemPriceDataProductDataOptions
                                    {
                                        Name = product.Name, // Product name from the Product model
                                    },
                                },
                                Quantity = cartItem.Quantity, // The quantity of this product in the session
                            });
                        }
                    }

                    lineItems.Add(new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(model.TaxAmount * 100), // Amount in cents
                            Currency = "cad",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "Tax(13%)",
                            },
                        },
                        Quantity = 1,
                    });

                    lineItems.Add(new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(model.DeliveryAmount * 100), // Amount in cents
                            Currency = "cad",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "Delivery charge",
                            },
                        },
                        Quantity = 1,
                    });


                    // Create a new Session using Stripe's SessionCreateOptions
                    var options = new SessionCreateOptions
                    {
                        PaymentMethodTypes = new List<string>
                        {
                            "card"
                        },
                        LineItems = lineItems, // Use the list of line items we created
                        Mode = "payment",
                        SuccessUrl = "https://localhost:7153/Checkout/ThankYou", // Redirect URL after successful payment
                        CancelUrl = "https://localhost:7153/Checkout/Index", // Redirect URL if the payment is canceled
                    };

                    var service = new SessionService();
                    var session = service.Create(options);



                    // Redirect the user to the Stripe hosted payment page
                    return Redirect(session.Url);
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that might occur during the database operations or payment processing
                ModelState.AddModelError("", "An error occurred while processing your order. Please try again later.");
            }

            var cart = GetCartItems();
            if (cart.Count == 0)
            {
                TempData["Message"] = "Please add products to the cart before checkout.";
                return RedirectToAction("Index", "Cart");
            }

            model.CartItems = cart;

            // Calculate the order summary values and update the model
            CalculateOrderSummary(model);

            return View("Index", model);
        }


        public IActionResult ThankYou()
        {
            try
            {


                // Check if session data exists
                if (!ValidateRole())
                {
                    // Redirect to the login page if session data is missing
                    return RedirectToAction("Index", "Login");
                }

                // Get the UserId using the GetUserId() method
                int userId = GetUserId();


                // Retrieve the order ID from session
                int? orderId = HttpContext.Session.GetInt32("OrderId");

                if (orderId == null)
                {
                    // If the orderId is null, handle the situation appropriately (e.g., redirect to an error page)
                    return RedirectToAction("Index", "Error");
                }

                // Retrieve the order from the database using orderId
                var order = _dbContext.Order.FirstOrDefault(o => o.UserId == userId && o.Id == orderId);

                if (order == null)
                {
                    // If the order is null, handle the situation appropriately (e.g., redirect to an error page)
                    return RedirectToAction("Index", "Error");
                }

                // Update the order in the database to set IsPaymentSuccess to true
                order.IsPaymentSuccess = true;
                _dbContext.SaveChanges();

                // Retrieve the cart items for the specific user from the "Cart" table
                var cartItems = _dbContext.Cart.Where(cart => cart.UserId == userId).ToList();

                // Save the order items to the database
                foreach (var cartItem in cartItems)
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
                        // Update the stock quantity of the product in the "Products" table
                        product.StockQuantity -= cartItem.Quantity;
                    }
                }

                _dbContext.SaveChanges();

                // Clear the cart
                ClearCart();


                // Redirect to the thank you page or perform any additional actions
                return View();
            }
            catch (Exception ex)
            {
                return null;
                
            }
        }


        //public IActionResult ThankYou()
        //{ 
        //    // Check if session data exists
        //    if (!ValidateRole())
        //    {
        //        // Redirect to login page if session data is missing
        //        return RedirectToAction("Index", "Login");
        //    }

        //    return View();
        //}

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

