using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MvcJs.Models;
using MvcJs.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MvcJs.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private static List<ProductInfoViewModel> _products = new()
            {
                new() { Name = "Ореховая смесь", Description = "Питательные бобы" },
                new() { Name = "Старый хлеб", Description = "Им можно убить кого-то" },
                new() { Name = "Тушенка", Description = "Завтрак туриста" },
            };
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult GetProductInfo()
        {
            return Json(new
            {
                Name = "Ореховая смесь",
                Description = "Питательные бобы восполняют жизненную энергию"
            });
        }
        [HttpPost]
        public IActionResult GetAllProducts([FromBody] ProductSearchFilter filter)
        {
            if (filter == null)
            {
                return BadRequest("Ошибка на стороне сервера");
            }
            var products = _products.Where(x => x.Name.Contains(filter.Name, StringComparison.CurrentCultureIgnoreCase))
                .OrderBy(x => x.Name).ToList();

            return Json(products);
        }
        [HttpPost]
        public IActionResult CreateProductInfo([FromBody] ProductInfoViewModel viewModel)
        {
            if (viewModel == null)
            {
                return BadRequest("Ошибка на стороне сервера");
            }

            if (viewModel.Name.Length < 3 || string.IsNullOrWhiteSpace(viewModel.Name))
            {
                return BadRequest("Название товара не может быть пустым");
            }
            _products.Add(viewModel);
            return Ok();
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
