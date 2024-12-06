using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ShoesForFeet.Controllers
{
    public class HomeController : Controller
    {
        // Display the home page
        public IActionResult Index()
        {
            // Add a welcome message for logged-in users
            if (User.Identity.IsAuthenticated)
            {
                ViewData["WelcomeMessage"] = $"Welcome, {User.Identity.Name}!";
            }
            return View();
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
    }
}