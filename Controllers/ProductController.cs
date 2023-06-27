using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GlamourHub.Models;
using GlamourHub.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GlamourHub.Controllers
{
    public class ProductController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationDbContext _dbContext;

        public ProductController(IWebHostEnvironment webHostEnvironment, ApplicationDbContext dbContext)
        {
            _webHostEnvironment = webHostEnvironment;
            _dbContext = dbContext;
        }

        // GET
        public IActionResult AddProduct()
        {
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
            Product existingProduct = _dbContext.Products.Find(id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            ViewBag.Categories = new SelectList(_dbContext.Categories, "Id", "Name");
            ViewBag.Brands = new SelectList(_dbContext.Brands, "Id", "Name");

            return View(existingProduct);
        }



        // POST
        [HttpPost]
        public IActionResult EditProduct(Product product, IFormFile newImage)
        {
            if (ModelState.IsValid)
            {
                Product existingProduct = _dbContext.Products.Find(product.Id);
                if (existingProduct == null)
                {
                    return NotFound();
                }

                // Delete the old image file if a new image is uploaded
                if (newImage != null)
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
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(newImage.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    newImage.CopyTo(new FileStream(filePath, FileMode.Create));
                    product.ImagePath = uniqueFileName;
                }
                else
                {
                    // If no new image is uploaded, retain the existing image path
                    product.ImagePath = existingProduct.ImagePath;
                }

                // Update other properties of the product
                existingProduct.Name = product.Name;
                existingProduct.Description = product.Description;
                existingProduct.Price = product.Price;
                existingProduct.CategoryId = product.CategoryId;
                existingProduct.BrandId = product.BrandId;

                _dbContext.SaveChanges();

                return RedirectToAction("ProductList");
            }

            // If the model is not valid, return the view with the validation errors
            return View(product);
        }

        // GET
        public IActionResult DeleteProduct(int id)
        {
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
            var productList = _dbContext.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .ToList();

            return View(productList);
        }
    }
}
