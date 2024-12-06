using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using ShoesForFeet.Models;
using ShoesForFeet.Data;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;

namespace ShoesForFeet.Controllers
{
    [Route("User")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Inject the database context
        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("Login")]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string Name, string Password)
        {
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Password))
            {
                ModelState.AddModelError("", "Both username and password are required.");
                return View();
            }

            // Validate user credentials without hashing
            var existingUser = _context.Users.FirstOrDefault(u => u.Name == Name && u.Password == Password);
            if (existingUser != null)
            {
                // Create user claims
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, existingUser.Name),
                    new Claim(ClaimTypes.Role, existingUser.Role)
                };

                // Create claims identity and principal
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                // Sign in user
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                TempData["Message"] = "Login successful!";
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid username or password.");
            return View();
        }

        [HttpGet]
        [Route("Register")]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("Register")]
        [AllowAnonymous]
        public IActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                // Check if username is already taken
                if (_context.Users.Any(u => u.Name == user.Name))
                {
                    ModelState.AddModelError("", "Username already exists.");
                    return View(user);
                }

                // Assign default role and save password as plaintext
                user.Role = "User";

                // Add user to the database
                _context.Users.Add(user);
                _context.SaveChanges();

                TempData["Message"] = "Registration successful! Please log in.";
                return RedirectToAction("Login");
            }

            return View(user);
        }

        [HttpGet]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            TempData["Message"] = "You have been logged out.";
            return RedirectToAction("Index", "Home");
        }
    }
}
