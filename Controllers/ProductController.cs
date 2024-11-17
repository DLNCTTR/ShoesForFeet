using Microsoft.AspNetCore.Mvc;
using ShoesForFeet.Models;
using System.Collections.Generic;

namespace ShoesForFeet.Controllers
{
    public class ProductController : Controller
    {
        private static List<Product> _products = new()
        {
            new Product { Id = 1, Name = "Running Shoes", ShoeSize = 10, Price = 79.99m },
            new Product { Id = 2, Name = "Casual Sneakers", ShoeSize = 9, Price = 49.99m }
        };


        public IActionResult List()
        {
            return View(_products);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Product product)
        {
            if (ModelState.IsValid)
            {
                product.Id = _products.Count + 1;
                _products.Add(product);
                return RedirectToAction("List");
            }
            return View(product);
        }
    }
}