using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ShoesForFeet.Data;
using System.Linq;

namespace ShoesForFeet.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Display the home page
        public IActionResult Index()
        {
            // Retrieve the "Best Sellers" (first 3 products) from the database
            var bestSellers = _context.Products.Take(3).ToList();

            // Add a welcome message for logged-in users
            if (User.Identity.IsAuthenticated)
            {
                ViewData["WelcomeMessage"] = $"Welcome, {User.Identity.Name}!";
            }

            // Pass the products to the view
            return View(bestSellers);
        }

        // Display the contact page (restricted to authenticated users)
        [Authorize]
        public IActionResult Contact()
        {
            // Add contact information to ViewData for the Contact view
            ViewData["ContactEmail"] = "support@shoesforfeet.com";
            ViewData["ContactPhone"] = "+1-800-555-1234";
            ViewData["ContactAddress"] = "123 Shoe Street, Foot City, USA";
            return View();
        }

        // Handle errors, including 404 errors
        [AllowAnonymous]
        public IActionResult Error(int statusCode)
        {
            if (statusCode == 404)
            {
                return View("Error404");
            }
            return View("Error");
        }

        // Custom action to display an error for Contact Us outside of support hours
        public IActionResult ContactUsError()
        {
            return View("ContactError");
        }
    }
}