using Microsoft.AspNetCore.Mvc;
using ShoesForFeet.Models;
using System.Collections.Generic;

namespace ShoesForFeet.Controllers
{
    public class ProductController : Controller
    {
        // Updated to allow image URL and support a better presentation
        private static List<Product> _products = new()
        {
            new Product { Id = 1, Name = "Running Shoes", ShoeSize = 10, Price = 79.99m, ImageUrl = "/images/running_shoes.jpg" },
            new Product { Id = 2, Name = "Casual Sneakers", ShoeSize = 9, Price = 49.99m, ImageUrl = "/images/casual_sneakers.jpg" }
        };

        // List all products with images
        public IActionResult List()
        {
            return View(_products);
        }

        // Display the form to add a new product
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        // Handle the form submission to add a new product
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

        // Get details of a specific product (added for better UX)
        [HttpGet]
        [Route("Product/Details/{id}")]
        public IActionResult Details(int id)
        {
            var product = _products.Find(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // Custom route added for showcasing routing improvement
        [HttpGet("Product/Specials")] // Attribute routing example
        public IActionResult Specials()
        {
            // Example logic for special products, could be filtered by price or promotions
            var specials = _products.FindAll(p => p.Price < 60);
            return View(specials);
        }
    }
} 
