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
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly ProductService _productService;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, ProductService productService)
        {
            _logger = logger;
            _context = context;
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
        public async Task<IActionResult> GetProducts()
        {
            var draw = HttpContext.Request.Query["draw"].FirstOrDefault();
            var start = HttpContext.Request.Query["start"].FirstOrDefault();
            var length = HttpContext.Request.Query["length"].FirstOrDefault();
            var searchValue = HttpContext.Request.Query["search[value]"].FirstOrDefault();
            var searchColumn = HttpContext.Request.Query["search[column]"].FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            // Panggil service untuk mengambil data
            var jsonData = await _productService.GetProductsAsync(skip, pageSize, searchValue, searchColumn, draw);
            return Json(jsonData);
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
