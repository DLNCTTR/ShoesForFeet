using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ShoesForFeet.Models;
using ShoesForFeet.Data;
using System.Linq;

namespace ShoesForFeet.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Constructor to inject the database context
        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Displays a list of all products
        [AllowAnonymous]
        public IActionResult List()
        {
            var products = _context.Products.ToList();
            return View(products);
        }

        // API endpoint to fetch search results dynamically
        [HttpGet]
        [AllowAnonymous]
        public JsonResult Search(string query)
        {
            var products = _context.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query))
            {
                products = products.Where(p =>
                    p.Name.Contains(query, System.StringComparison.OrdinalIgnoreCase) ||
                    p.Description.Contains(query, System.StringComparison.OrdinalIgnoreCase));
            }

            return Json(products.ToList());
        }

        // Displays details of a specific product
        [HttpGet]
        [Route("Product/Details/{id}")]
        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
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
                _context.Products.Add(product);
                _context.SaveChanges();
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
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
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
            var product = _context.Products.FirstOrDefault(p => p.Id == updatedProduct.Id);
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

                _context.SaveChanges();
                return RedirectToAction("List");
            }
            return View(updatedProduct);
        }

        // Displays a list of special offers (products under a specific price)
        [HttpGet("Product/Specials")]
        [AllowAnonymous]
        public IActionResult Specials()
        {
            var specials = _context.Products.Where(p => p.Price < 60).ToList();
            return View(specials);
        }
    }
}
