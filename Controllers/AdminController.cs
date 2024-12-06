using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoesForFeet.Data;
using ShoesForFeet.Models;

namespace ShoesForFeet.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ProductRepository _productRepository;

        public AdminController(ProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public IActionResult Dashboard()
        {
            var products = _productRepository.GetAllProducts();
            return View(products);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Product product)
        {
            if (ModelState.IsValid)
            {
                _productRepository.AddProduct(product);
                return RedirectToAction("Dashboard");
            }
            return View(product);
        }
    }
}