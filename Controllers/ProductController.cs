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

        // Displays the form to add a new product
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Add()
        {
            return View(); // Returns the Add form view
        }

        // Handles the submission of the form to add a new product
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Add(Product product)
        {
            if (ModelState.IsValid) // Checks for validation errors
            {
                product.Id = _products.Count + 1; // Assigns a new unique ID
                _products.Add(product); 
                return RedirectToAction("List"); // Redirects to the List view
            }
            return View(product); 
        }

        // Displays the form to edit details of a specific product
        [HttpGet]
        [Route("Product/Details/{id}")] // Attribute route for accessing product details
        [Authorize(Roles = "Admin")]
        public IActionResult Details(int id)
        {
            var product = _products.Find(p => p.Id == id); // Finds the product by ID
            if (product == null) // If no product is found, return a 404 page
            {
                return NotFound(); // Returns a 404 error page
            }
            return View(product); 
        }

        // Handles the submission of the form to update product details
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Details(Product updatedProduct)
        {
            var product = _products.Find(p => p.Id == updatedProduct.Id); // Finds the existing product
            if (product == null)
            {
                return NotFound(); // Returns a 404 error page if the product doesn't exist
            }

            if (ModelState.IsValid) // Checks for validation errors
            {
                // Updates product properties
                product.Name = updatedProduct.Name;
                product.ShoeSize = updatedProduct.ShoeSize;
                product.Price = updatedProduct.Price;
                product.ImageUrl = updatedProduct.ImageUrl;
                product.Description = updatedProduct.Description;

                return RedirectToAction("List"); // Redirects to the List view after successful update
            }

            return View(updatedProduct); // Re-displays the form with validation errors
        }

        // Displays a list of special offers (products under a specific price)
        [HttpGet("Product/Specials")]
        [AllowAnonymous]
        public IActionResult Specials()
        {
            var specials = _products.FindAll(p => p.Price < 60); // Filters products with a price below $60
            return View(specials); 
        }
    }
}