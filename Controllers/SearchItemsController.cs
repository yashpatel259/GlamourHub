using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using GlamourHub.DataAccess;
using GlamourHub.Models;

namespace GlamourHub.Controllers
{
    public class SearchItemsController : Controller
    {
        private readonly GlamourHubContext _dbContext;

        public SearchItemsController(GlamourHubContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult SearchItems(string searchQuery)
        {
            // Perform the search based on the "searchQuery" parameter
            if (!string.IsNullOrEmpty(searchQuery))
            {
                // Get the products that match the search query using EF.Functions.Like
                var matchingProducts = _dbContext.Products
                    .Where(p => EF.Functions.Like(p.Name, $"%{searchQuery}%") || EF.Functions.Like(p.Description, $"%{searchQuery}%") || EF.Functions.Like(p.Category.Name, $"%{searchQuery}%") || EF.Functions.Like(p.Brand.Name, $"%{searchQuery}%"))
                    .ToList();

                // Fetch product names and brand names from the database
                var productNames = _dbContext.GetProductNames();
                var brandNames = _dbContext.GetBrandNames();

                // Combine product names and brand names to create the data array
                var data = productNames.Concat(brandNames).ToList();

                // Pass the matching products to the view
                return View(matchingProducts);
            }

            // If the search query is empty or null, return an empty view
            return View(new List<Product>());
        }
    }
}
