using GlamourHub.DataAccess;
using GlamourHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GlamourHub.Controllers
{
    
    public class CategoryController : Controller
    {
        private readonly GlamourHubContext _dbContext;

        public CategoryController(GlamourHubContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CategoryList(int? page)
        {
            try
            {
                // Check if session data exists
                if (!ValidateRole())
                {
                    // Redirect to login page if session data is missing
                    return RedirectToAction("Index", "Login");
                }

                int pageSize = 10; // Change this to the desired page size
                int pageNumber = page ?? 1;

                var categories = _dbContext.Categories.ToList();

                var pagedCategories = categories.Skip((pageNumber - 1) * pageSize).Take(pageSize);

                ViewBag.CurrentPage = pageNumber;
                ViewBag.TotalPages = (int)Math.Ceiling((double)categories.Count() / pageSize);

                return View(pagedCategories);
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        [HttpGet]
        public IActionResult AddCategory()
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
        public IActionResult AddCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                // Check if the category name already exists
                bool isCategoryExists = _dbContext.Categories.Any(c => c.Name == category.Name);
                if (isCategoryExists)
                {
                    ModelState.AddModelError("Name", "Category name already exists.");
                    return View(category);
                }
                category.CreatedAt = DateTime.Now;
                _dbContext.Categories.Add(category);
                _dbContext.SaveChanges();

                //TempData["SuccessMessage"] = "Category added successfully!";
                //return View(category);
                return RedirectToAction("CategoryList");
            }

            return View(category);
        }

        [HttpGet]
        public IActionResult EditCategory(int id)
        {
            // Check if session data exists
            if (!ValidateRole())
            {
                // Redirect to login page if session data is missing
                return RedirectToAction("Index", "Login");
            }
            // Retrieve the existing category from the database
            Category existingCategory = _dbContext.Categories.Find(id);
            if (existingCategory == null)
            {
                return NotFound();
            }

            return View(existingCategory);
        }

        [HttpPost]
        public IActionResult EditCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                // Retrieve the existing category from the database
                Category existingCategory = _dbContext.Categories.Find(category.Id);
                if (existingCategory == null)
                {
                    return NotFound();
                }

                // Check if the updated category name already exists (excluding the current category)
                bool isCategoryExists = _dbContext.Categories.Any(c => c.Name == category.Name && c.Id != category.Id);
                if (isCategoryExists)
                {
                    ModelState.AddModelError("Name", "Category name already exists.");
                    return View(category);
                }

                // Update the properties of the existing category
                existingCategory.Name = category.Name;
                existingCategory.CreatedAt = DateTime.Now;

                _dbContext.SaveChanges();

                return RedirectToAction("CategoryList");
            }

            return View(category);
        }

        [HttpPost]
        public IActionResult DeleteCategory(int id)
        {
            Category existingCategory = _dbContext.Categories.Find(id);
            if (existingCategory == null)
            {
                return NotFound();
            }

            return View(existingCategory);
        }

        [HttpPost]
        public IActionResult ConfirmDeleteCategory(int id)
        {
            Category existingCategory = _dbContext.Categories.Find(id);
            if (existingCategory == null)
            {
                return NotFound();
            }

            _dbContext.Categories.Remove(existingCategory);
            _dbContext.SaveChanges();

            return RedirectToAction("CategoryList");
        }


        //[HttpPost]
        //public IActionResult DeleteCategory(int id)
        //{
        //    // Find the category by its ID
        //    Category category = _dbContext.Categories.Find(id);
        //    if (category == null)
        //    {
        //        return NotFound();
        //    }

        //    _dbContext.Categories.Remove(category);
        //    _dbContext.SaveChanges();

        //    return RedirectToAction("CategoryList");
        //}

        public bool ValidateRole()
        {
            return HttpContext.Session.GetString("Role") == "Admin" ? true : false;
        }

    }
}
