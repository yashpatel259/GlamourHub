using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using GlamourHub.Models;
using GlamourHub.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace GlamourHub.Controllers
{
  
    public class CartController : Controller
    {
        private readonly GlamourHubContext _dbContext;

        public CartController(GlamourHubContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: /Cart
        public IActionResult Index()
        {
            // Check if session data exists
            if (!ValidateRole())
            {
                // Redirect to login page if session data is missing
                return RedirectToAction("Index", "Login");
            }
            var cart = GetCartItems();
            return View(cart);
        }

        // POST: /Cart/AddToCart
        [HttpPost]
        public IActionResult AddToCart(int productId,int quantity)
        {
            var product = _dbContext.Products.Find(productId);
            if (product == null)
            {
                return NotFound();
            }

            var cart = GetCartItems();

            var existingItem = cart.FirstOrDefault(item => item.ProductId == productId);
            if (existingItem != null)
            {
                existingItem.Quantity = existingItem.Quantity+ quantity;
            }
            else
            {
                var newItem = new Cart
                {
                    ProductId = product.Id,
                    Quantity = quantity
                };
                cart.Add(newItem);
            }

            SaveCartItems(cart);

            return RedirectToAction("Index", "Cart");
        }


        // POST: /Cart/RemoveFromCart
        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            var cart = GetCartItems();

            var item = cart.FirstOrDefault(item => item.ProductId == productId);
            if (item != null)
            {
                cart.Remove(item);
                SaveCartItems(cart);
            }

            return RedirectToAction("Index", "Cart");
        }

       

        private List<Cart> GetCartItems()
        {
            var userId = GetUserId();
            return _dbContext.Cart.Include(cart => cart.Product).Where(cart => cart.UserId == userId).ToList();
        }

        private void SaveCartItems(List<Cart> cart)
        {
            var userId = GetUserId();

            // Get the existing cart items for the user
            var existingItems = _dbContext.Cart.Where(cart => cart.UserId == userId).ToList();

            // Update existing items and remove items that are no longer in the cart
            foreach (var existingItem in existingItems)
            {
                var newItem = cart.FirstOrDefault(item => item.ProductId == existingItem.ProductId);
                if (newItem != null)
                {
                    // Update the quantity
                    existingItem.Quantity = newItem.Quantity;
                    cart.Remove(newItem); // Remove the item from the cart list to avoid duplicates
                }
                else
                {
                    // The item was removed from the cart, so remove it from the database
                    _dbContext.Cart.Remove(existingItem);
                }
            }

            // Add new cart items
            foreach (var newItem in cart)
            {
                newItem.UserId = userId;
                _dbContext.Cart.Add(newItem);
            }

            _dbContext.SaveChanges();
        }

        public int GetTotalProductsInCart(string username)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Username == username);

            int userId = 0;
            if (user != null)
            {
                userId = user.Id;
            }

            // Fetch cart items for the current user
            var cartItems = _dbContext.Cart
                .Where(ci => ci.UserId == userId)
                .ToList();

            // Calculate the total number of products in the cart
            int totalProductsInCart = cartItems.Sum(ci => ci.Quantity);

            return totalProductsInCart;
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
    }
}
