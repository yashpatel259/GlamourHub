using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GlamourHub.Models;
using GlamourHub.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace GlamourHub.Controllers
{
    public class ProductController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly GlamourHubContext _dbContext;

        public ProductController(IWebHostEnvironment webHostEnvironment, GlamourHubContext dbContext)
        {
            _webHostEnvironment = webHostEnvironment;
            _dbContext = dbContext;
        }

        // GET
        public IActionResult AddProduct()
        { 
            // Check if session data exists
            if (!ValidateRole())
            {
                // Redirect to login page if session data is missing
                return RedirectToAction("Index", "Login");
            }
            ViewBag.Categories = _dbContext.Categories; // Pass categories to the view for dropdown
            ViewBag.Brands = _dbContext.Brands; // Pass brands to the view for dropdown
            return View();
        }

        // POST
        [HttpPost]
        public IActionResult AddProduct(Product product, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    imageFile.CopyTo(new FileStream(filePath, FileMode.Create));
                    product.ImagePath = uniqueFileName;
                }

                product.CreatedAt = DateTime.Now;
                _dbContext.Products.Add(product);
                _dbContext.SaveChanges();

                return RedirectToAction("ProductList");
            }

            ViewBag.Categories = _dbContext.Categories; // Pass categories to the view for dropdown
            ViewBag.Brands = _dbContext.Brands; // Pass brands to the view for dropdown
            return View(product);
        }

        // GET
        public IActionResult EditProduct(int id)
        {
            // Check if session data exists
            if (!ValidateRole())
            {
                // Redirect to login page if session data is missing
                return RedirectToAction("Index", "Login");
            }
            Product existingProduct = _dbContext.Products.Find(id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            ViewBag.Categories = new SelectList(_dbContext.Categories, "Id", "Name", existingProduct.CategoryId);
            ViewBag.Brands = new SelectList(_dbContext.Brands, "Id", "Name", existingProduct.BrandId);

            return View(existingProduct);
        }

        // POST
        [HttpPost]
        public IActionResult EditProduct(Product product, IFormFile? strImage)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                // Inspect the errors and debug the issue
            }

            if (ModelState.IsValid)
            {
                Product existingProduct = _dbContext.Products.Find(product.Id);
                if (existingProduct == null)
                {
                    return NotFound();
                }

                // Delete the old image file if a new image is uploaded
                if (strImage != null)
                {
                    if (!string.IsNullOrEmpty(existingProduct.ImagePath))
                    {
                        string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", existingProduct.ImagePath);
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }

                    // Save the new image file
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(strImage.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    strImage.CopyTo(new FileStream(filePath, FileMode.Create));
                    product.ImagePath = uniqueFileName;
                }
                else
                {
                    // If no new image is uploaded, retain the existing image path
                    product.ImagePath = existingProduct.ImagePath;
                }

                // Update other properties of the product
                existingProduct.Name = product.Name;
                existingProduct.ImagePath = product.ImagePath;
                existingProduct.Description = product.Description;
                existingProduct.Price = product.Price;
                existingProduct.CategoryId = product.CategoryId;
                existingProduct.BrandId = product.BrandId;
                existingProduct.IsSale = product.IsSale;
                existingProduct.StockQuantity = product.StockQuantity;
                _dbContext.SaveChanges();

                return RedirectToAction("ProductList");
            }

            // If the model is not valid, return the view with the validation errors
            ViewBag.Categories = new SelectList(_dbContext.Categories, "Id", "Name", product.CategoryId);
            ViewBag.Brands = new SelectList(_dbContext.Brands, "Id", "Name", product.BrandId);
            return View(product);
        }

        // GET
        public IActionResult DeleteProduct(int id)
        {
            // Check if session data exists
            if (!ValidateRole())
            {
                // Redirect to login page if session data is missing
                return RedirectToAction("Index", "Login");
            }
            Product existingProduct = _dbContext.Products.Find(id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            return View(existingProduct);
        }

        // POST
        [HttpPost]
        public IActionResult ConfirmDeleteProduct(int id)
        {
            Product existingProduct = _dbContext.Products.Find(id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(existingProduct.ImagePath))
            {
                string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", existingProduct.ImagePath);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _dbContext.Products.Remove(existingProduct);
            _dbContext.SaveChanges();

            return RedirectToAction("ProductList");
        }

        // GET
        public IActionResult ProductList()
        {
            // Check if session data exists
            if (!ValidateRole())
            {
                // Redirect to login page if session data is missing
                return RedirectToAction("Index", "Login");
            }
            var productList = _dbContext.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .ToList();

            return View(productList);
        }

        [AllowAnonymous]
        public IActionResult Shop(int? page, string categoryFilter, string brandFilter, string priceFilter)
        {
            int pageSize = 16; // Number of products per page

            // Get all available categories and brands from the database
            var allCategories = _dbContext.Categories.ToList();
            var allBrands = _dbContext.Brands.ToList();

            // Pass the lists of categories and brands to the view using ViewBag
            ViewBag.AllCategories = allCategories;
            ViewBag.AllBrands = allBrands;

            // Get all products from the database, including related data (Category and Brand)
            IQueryable<Product> query = _dbContext.Products
                .Include(p => p.Category)
                .Include(p => p.Brand);

            // Apply filters if provided
            if (!string.IsNullOrEmpty(categoryFilter))
            {
                int categoryId = int.Parse(categoryFilter);
                query = query.Where(p => p.CategoryId == categoryId);
            }

            if (!string.IsNullOrEmpty(brandFilter))
            {
                int brandId = int.Parse(brandFilter);
                query = query.Where(p => p.BrandId == brandId);
            }

            if (!string.IsNullOrEmpty(priceFilter))
            {
                switch (priceFilter)
                {
                    case "50":
                        query = query.Where(p => p.Price <= 50);
                        break;
                    case "51-99":
                        query = query.Where(p => p.Price >= 51 && p.Price <= 99);
                        break;
                    case "100-499":
                        query = query.Where(p => p.Price >= 100 && p.Price <= 499);
                        break;
                    case "500":
                        query = query.Where(p => p.Price >= 500);
                        break;
                }
            }

            // Get the total count of filtered products
            int totalCount = query.Count();

            // Paginate the filtered products using the PaginatedList class
            var paginatedProducts = PaginatedList<Product>.Create(query, page ?? 1, pageSize);

            // Create a ViewModel that holds the paginated products and the filter parameters
            var viewModel = new ShopViewModel
            {
                Products = paginatedProducts,
                CategoryFilter = categoryFilter,
                BrandFilter = brandFilter,
                PriceFilter = priceFilter,
                TotalCount = totalCount
            };

            return View(viewModel);
        }

        // GET: /Product/Details/{id}
        [AllowAnonymous]
        public IActionResult ProductDetails(int id)
        {
            var product = _dbContext.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        public bool ValidateRole()
        {
            return HttpContext.Session.GetString("Role") == "Admin" || HttpContext.Session.GetString("Role") == "Seller" ? true : false;
        }
    }
}
