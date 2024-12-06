using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoesForFeet.Data;
using ShoesForFeet.Models;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace ShoesForFeet.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Admin Dashboard: Displays a list of all products
        public IActionResult Dashboard()
        {
            var products = _context.Products.ToList();
            return View(products);
        }

        // Display Add Product form
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        // Handle Add Product form submission
        [HttpPost]
        public IActionResult Add(Product product, IFormFile? ImageFile)
        {
            if (ModelState.IsValid)
            {
                // Handle optional image upload
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    var fileName = Path.GetFileName(ImageFile.FileName);
                    var filePath = Path.Combine("wwwroot/Images/Products", fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        ImageFile.CopyTo(stream);
                    }
                    product.ImageUrl = $"/Images/Products/{fileName}";
                }

                _context.Products.Add(product);
                _context.SaveChanges();
                TempData["Message"] = "Product added successfully!";
                return RedirectToAction("Dashboard");
            }
            return View(product);
        }

        // Display Edit Product form
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return RedirectToAction("Dashboard");
            }
            return View(product);
        }

        // Handle Edit Product form submission
        [HttpPost]
        public IActionResult Edit(Product updatedProduct, IFormFile? ImageFile)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == updatedProduct.Id);
            if (product == null)
            {
                return RedirectToAction("Dashboard");
            }

            if (ModelState.IsValid)
            {
                // Update product details
                product.Name = updatedProduct.Name;
                product.ShoeSize = updatedProduct.ShoeSize;
                product.Price = updatedProduct.Price;
                product.Description = updatedProduct.Description;

                // Handle optional image upload
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    var fileName = Path.GetFileName(ImageFile.FileName);
                    var filePath = Path.Combine("wwwroot/Images/Products", fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        ImageFile.CopyTo(stream);
                    }
                    product.ImageUrl = $"/Images/Products/{fileName}";
                }

                _context.SaveChanges();
                TempData["Message"] = "Product updated successfully!";
                return RedirectToAction("Dashboard");
            }
            return View(updatedProduct);
        }

        // Handle Delete Product action
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
                TempData["Message"] = "Product deleted successfully!";
            }
            return RedirectToAction("Dashboard");
        }
    }
}
