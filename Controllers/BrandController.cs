using System.Linq;
using GlamourHub.DataAccess;
using GlamourHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


public class BrandController : Controller
{
    private readonly GlamourHubContext _dbContext;

    public BrandController(GlamourHubContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IActionResult BrandList(int? page)
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

            var brands = _dbContext.Brands.ToList();

            var pagedBrands = brands.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalPages = (int)Math.Ceiling((double)brands.Count() / pageSize);

            return View(pagedBrands);
        }
        catch (Exception ex)
        {
            return null;
        }
    }


    [HttpGet]
    public IActionResult AddBrand()
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
    public IActionResult AddBrand(Brand brand)
    {
        if (ModelState.IsValid)
        {
            // Check if the category name already exists
            bool isBrandExists = _dbContext.Brands.Any(c => c.Name == brand.Name);
            if (isBrandExists)
            {
                ModelState.AddModelError("Name", "Brand name already exists.");
                return View(brand);
            }

            _dbContext.Brands.Add(brand);
            _dbContext.SaveChanges();
            brand.CreatedAt = DateTime.Now;
            return RedirectToAction("BrandList");
        }

        return View(brand);
    }

    [HttpGet]
    public IActionResult EditBrand(int id)
    {
        // Check if session data exists
        if (!ValidateRole())
        {
            // Redirect to login page if session data is missing
            return RedirectToAction("Index", "Login");
        }

        Brand existingBrand = _dbContext.Brands.Find(id);
        if (existingBrand == null)
        {
            return NotFound();
        }

        return View(existingBrand);
    }

    [HttpPost]
    public IActionResult EditBrand(Brand brand)
    {
        if (ModelState.IsValid)
        {
            Brand existingBrand = _dbContext.Brands.Find(brand.Id);
            if (existingBrand == null)
            {
                return NotFound();
            }

            existingBrand.Name = brand.Name;
            _dbContext.SaveChanges();

            return RedirectToAction("BrandList");
        }

        return View(brand);
    }

    [HttpGet]
    public IActionResult DeleteBrand(int id)
    {
        // Check if session data exists
        if (!ValidateRole())
        {
            // Redirect to login page if session data is missing
            return RedirectToAction("Index", "Login");
        }
        Brand existingBrand = _dbContext.Brands.Find(id);
        if (existingBrand == null)
        {
            return NotFound();
        }

        return View(existingBrand);
    }

    [HttpPost]
    public IActionResult ConfirmDeleteBrand(int id)
    {
        Brand existingBrand = _dbContext.Brands.Find(id);
        if (existingBrand == null)
        {
            return NotFound();
        }

        _dbContext.Brands.Remove(existingBrand);
        _dbContext.SaveChanges();

        return RedirectToAction("BrandList");
    }

    public bool ValidateRole()
    {
        return HttpContext.Session.GetString("Role") == "Admin" ? true : false;
    }
}
