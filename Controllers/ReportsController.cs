using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using AspNetCore.Reporting;
using Microsoft.EntityFrameworkCore;
using GlamourHub.Models;

namespace GlamourHub.Controllers
{
    public class ReportsController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly GlamourHubContext _dbContext;

        public ReportsController(IWebHostEnvironment webHostEnvironment, GlamourHubContext dbContext)
        {
            this._webHostEnvironment = webHostEnvironment;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            _dbContext = dbContext;
        }
               
        public IActionResult Print()
        {
            try
            {
                // Check if session data exists
                if (!ValidateRole())
                {
                    // Redirect to login page if session data is missing
                    return RedirectToAction("Index", "Login");
                }

                string mimtype = "";
                int extension = 1;
                // Call the stored procedure and get the order details
                List<OrderDetail> orderDetails = _dbContext.GetOrderDetails();

                var path = $"{this._webHostEnvironment.WebRootPath}\\Reports\\Orders.rdlc";
                Dictionary<string, string> parameters = new Dictionary<string, string>();

                LocalReport localReport = new LocalReport(path);
                localReport.AddDataSource("DataSet1", orderDetails);
                var result = localReport.Execute(RenderType.Pdf, extension, parameters, mimtype);
                return File(result.MainStream, "application/pdf");
                ;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //public IActionResult OrderReport()
        //{
        //    try
        //    {
        //        // Check if session data exists
        //        if (!ValidateRole())
        //        {
        //            // Redirect to login page if session data is missing
        //            return RedirectToAction("Index", "Login");
        //        }

        //        // Call the stored procedure and get the order details
        //        List<OrderDetail> orderDetails = _dbContext.GetOrderDetails();

        //        // Pass the data to the view
        //        return View(orderDetails);
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        public IActionResult OrderReport(int page = 1, int pageSize = 10)
        {
            try
            {
                // Check if session data exists and validate role

                // Call the stored procedure and get the order details
                List<OrderDetail> allOrderDetails = _dbContext.GetOrderDetails();

                // Calculate the number of pages
                int totalPages = (int)Math.Ceiling(allOrderDetails.Count / (double)pageSize);

                // Ensure the page number is within valid range
                page = Math.Max(1, Math.Min(totalPages, page));

                // Get the relevant portion of order details for the current page
                List<OrderDetail> orderDetails = allOrderDetails.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                // Pass the data to the view along with pagination info
                ViewData["TotalPages"] = totalPages;
                ViewData["CurrentPage"] = page;
                ViewData["PageSize"] = pageSize;

                return View(orderDetails);
            }
            catch (Exception ex)
            {
                return null; // Handle the exception appropriately
            }
        }

        //public IActionResult ProductInventoryReport()
        //{
        //    try
        //    {
        //        // Check if session data exists
        //        if (!ValidateRole())
        //        {
        //            // Redirect to login page if session data is missing
        //            return RedirectToAction("Index", "Login");
        //        }

        //        // Call the stored procedure and get the product inventory details
        //        List<ProductInventory> productInventory = _dbContext.GetProductInventory();

        //        // Pass the data to the view
        //        return View(productInventory);
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        public IActionResult ProductInventoryReport(int page = 1, int pageSize = 10)
        {
            try
            {
                // Check if session data exists and validate role

                // Call the stored procedure and get the product inventory details
                List<ProductInventory> allProductInventory = _dbContext.GetProductInventory();

                // Calculate the number of pages
                int totalPages = (int)Math.Ceiling(allProductInventory.Count / (double)pageSize);

                // Ensure the page number is within valid range
                page = Math.Max(1, Math.Min(totalPages, page));

                // Get the relevant portion of product inventory for the current page
                List<ProductInventory> productInventory = allProductInventory.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                // Pass the data to the view along with pagination info
                ViewData["TotalPages"] = totalPages;
                ViewData["CurrentPage"] = page;
                ViewData["PageSize"] = pageSize;

                return View(productInventory);
            }
            catch (Exception ex)
            {
                return null; // Handle the exception appropriately
            }
        }

        public IActionResult PrintProductInventory()
        {
            try
            {
                // Check if session data exists
                if (!ValidateRole())
                {
                    // Redirect to login page if session data is missing
                    return RedirectToAction("Index", "Login");
                }

                string mimeType = "";
                int extension = 1;

                // Call the stored procedure and get the product inventory details
                List<ProductInventory> productInventory = _dbContext.GetProductInventory();

                var path = $"{this._webHostEnvironment.WebRootPath}\\Reports\\ProductInventory.rdlc";
                Dictionary<string, string> parameters = new Dictionary<string, string>();

                LocalReport localReport = new LocalReport(path);
                localReport.AddDataSource("DataSet1", productInventory);
                var result = localReport.Execute(RenderType.Pdf, extension, parameters, mimeType);

                return File(result.MainStream, "application/pdf");
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public bool ValidateRole()
        {
            return HttpContext.Session.GetString("Role") == "Admin" || HttpContext.Session.GetString("Role") == "Seller" ? true : false;
        }
    }
}

