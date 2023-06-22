using System.Linq;
using GlamourHub.DataAccess;
using GlamourHub.Models;
using Microsoft.AspNetCore.Mvc;

public class BrandController : Controller
{
    private readonly ApplicationDbContext _dbContext;

    public BrandController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IActionResult BrandList()
    {
        var brands = _dbContext.Brands.ToList();
        return View(brands);
    }

    [HttpGet]
    public IActionResult AddBrand()
    {
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
}
