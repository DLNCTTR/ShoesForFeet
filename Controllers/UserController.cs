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
        public async Task<IActionResult> Login(User user)
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.Name == user.Name && u.Password == user.Password);
            if (existingUser != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, existingUser.Name),
                    new Claim(ClaimTypes.Role, existingUser.Role)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                TempData["Message"] = "Login successful!";
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid username or password.");
            return View(user);
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
                // Check if username already exists
                if (_context.Users.Any(u => u.Name == user.Name))
                {
                    ModelState.AddModelError("", "Username already exists.");
                    return View(user);
                }

                // Save user to the database
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
