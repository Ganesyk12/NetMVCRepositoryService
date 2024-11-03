using System;

using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using new_pages.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using new_pages.Repositories;

namespace new_pages.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;   //monitoring log
        private readonly ProductService _productService;  // service pattern
        private readonly IRoleRepository _roleRepository;  // service pattern
        private readonly IDeptRepository _deptRepository;  // service pattern

        public HomeController(ILogger<HomeController> logger, ProductService productService, IRoleRepository roleRepository, IDeptRepository deptRepository)
        {
            _logger = logger;
            _productService = productService;
            _roleRepository = roleRepository;
            _deptRepository = deptRepository;
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
            var sortColumnIndex = HttpContext.Request.Query["order[0][column]"].FirstOrDefault();
            var sortDirection = HttpContext.Request.Query["order[0][dir]"].FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            var sortColumn = GetColumnName(sortColumnIndex);
            // Panggil service untuk mengambil data
            var (jsonData, filteredCount) = await _productService.GetAllProductsAsync(skip, pageSize, searchColumn, searchValue, sortColumn, sortDirection);
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

        private string GetColumnName(string sortColumnIndex)
        {
            return sortColumnIndex switch {
                "1" => "role_name",
                "2" => "nik",
                "3" => "username",
                "4" => "office_email",
                "5" => "kode_department",
                "6" => "nama_department",
            };
        }

        [HttpPost]
        public async Task<IActionResult> SaveUserData(User user)
        {
            await _productService.SaveUserDataAsync(user);
            return Json(new { message = "Data berhasil disimpan" });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleRepository.GetAllRolesAsync();
            return Json(roles.Select(role => new { id = role.role_id, text = role.role_name }));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDept(string? searchTerm)
        {
            var dept = await _deptRepository.GetAllDept();
            var filteredDept = string.IsNullOrEmpty(searchTerm) 
            ? dept 
            : dept.Where(d => d.dept_name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
            return Json(dept.Select(d => new { id = d.deptId, text = d.dept_name }));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteData(string hdrid)
        {
            if (string.IsNullOrEmpty(hdrid)) {
                return BadRequest(new { message = "ID tidak valid!" });
            } else {
                var user = await _productService.GetByIdAsync(hdrid);
                if (user == null) {
                    return NotFound(new { message = "Data tidak ditemukan!" });
                } else { // jika user tersedia
                    await _productService.DeleteAsync(hdrid); 
                    return Ok(new { message = "Data berhasil dihapus!" });
                }
            }
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
