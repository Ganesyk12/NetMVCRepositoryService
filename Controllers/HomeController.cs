using System;

using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using new_pages.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace new_pages.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;   //monitoring log
        private readonly ProductService _productService;  // service pattern
        public HomeController(ILogger<HomeController> logger, ProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts(string searchColumn, string searchValue)
        {
            var draw = HttpContext.Request.Query["draw"].FirstOrDefault();
            var start = HttpContext.Request.Query["start"].FirstOrDefault();
            var length = HttpContext.Request.Query["length"].FirstOrDefault();
            // var searchValue = HttpContext.Request.Query["search[value]"].FirstOrDefault();
            // var searchColumn = HttpContext.Request.Query["search[column]"].FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            // Panggil service untuk mengambil data
            var (jsonData, filteredCount) = await _productService.GetAllProductsAsync(skip, pageSize, searchColumn, searchValue);
            int recordTotal = await _productService.GetTotalRecordsAsync();

            return Json(new
                {
                    draw = draw,
                    recordsTotal = recordTotal,
                    recordsFiltered = filteredCount,
                    data = jsonData
                }
            );
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
