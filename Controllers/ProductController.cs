using Microsoft.AspNetCore.Mvc;
using ShoesForFeet.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace ShoesForFeet.Controllers
{
    // Handles product-related operations like listing, adding, editing, and viewing details
    [Authorize]
    public class ProductController : Controller
    {
        // Static list to simulate a database of products
        private static List<Product> _products = new()
        {
            new Product
            {
                Id = 1,
                Name = "Running Shoes",
                ShoeSize = 10,
                Price = 79.99m,
                ImageUrl = "/Images/Products/Runners.jpg",
                Description = "Comfortable running shoes designed for performance."
            },
            new Product
            {
                Id = 2,
                Name = "Casual Sneakers",
                ShoeSize = 9,
                Price = 49.99m,
                ImageUrl = "/Images/Products/DressShoes.jpg",
                Description = "Classy shoes for any event."
            },
            new Product
            {
                Id = 3,
                Name = "Boots",
                ShoeSize = 14,
                Price = 79.99m,
                ImageUrl = "/Images/Products/Boots.jpg",
                Description = "Work boots suitable for any terrain."
            }
        };

        // Displays a list of all products
        [AllowAnonymous]
        public IActionResult List()
        {
            return View(_products);
        }

        // Displays details of a specific product
        [HttpGet]
        [Route("Product/Details/{id}")]
        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            var product = _products.Find(p => p.Id == id); // Finds the product by ID
            if (product == null)
            {
                return RedirectToAction("ErrorPage", "Error");
            }
            return View(product);
        }

        // Displays the form to add a new product
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Add()
        {
            return View();
        }

        // Handles the submission of the form to add a new product
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Add(Product product)
        {
            if (ModelState.IsValid)
            {
                product.Id = _products.Count > 0 ? _products[^1].Id + 1 : 1; // Assigns a unique ID
                _products.Add(product);
                return RedirectToAction("List");
            }
            return View(product);
        }

        // Displays the form to edit an existing product
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("Product/Edit/{id}")]
        public IActionResult Edit(int id)
        {
            var product = _products.Find(p => p.Id == id);
            if (product == null)
            {
                return RedirectToAction("ErrorPage", "Error");
            }
            return View(product);
        }

        // Handles the form submission to update product details
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(Product updatedProduct)
        {
            var product = _products.Find(p => p.Id == updatedProduct.Id);
            if (product == null)
            {
                return RedirectToAction("ErrorPage", "Error");
            }

            if (ModelState.IsValid)
            {
                product.Name = updatedProduct.Name;
                product.ShoeSize = updatedProduct.ShoeSize;
                product.Price = updatedProduct.Price;
                product.ImageUrl = updatedProduct.ImageUrl;
                product.Description = updatedProduct.Description;
                return RedirectToAction("List");
            }
            return View(updatedProduct);
        }

        // Displays a list of special offers (products under a specific price)
        [HttpGet("Product/Specials")]
        [AllowAnonymous]
        public IActionResult Specials()
        {
            var specials = _products.FindAll(p => p.Price < 60);
            return View(specials);
        }

        // Search products by name or description
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return View(new List<Product>());
            }

            var results = _products.FindAll(p =>
                p.Name.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                p.Description.Contains(query, StringComparison.OrdinalIgnoreCase));
            return View(results);
        }
    }
}
