using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using ShoesForFeet.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShoesForFeet.Controllers
{
    [Route("User")]
    public class UserController : Controller
    {
        // Simulated user database
        private static List<User> _users = new()
        {
            new User { Id = 1, Name = "Admin", Password = "admin123", Role = "Admin" },
            new User { Id = 2, Name = "User1", Password = "user123", Role = "User" }
        };

        // Display the login page
        [HttpGet]
        [Route("Login")]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        // Handle login form submission
        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(User user)
        {
            var existingUser = _users.Find(u => u.Name == user.Name && u.Password == user.Password);
            if (existingUser != null)
            {
                // Create authentication claims
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, existingUser.Name),
                    new Claim(ClaimTypes.Role, existingUser.Role)
                };

                // Create claims identity and sign in
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                TempData["Message"] = "Login successful!";
                return RedirectToAction("Index", "Home");
            }

            // Add error message for invalid credentials
            ModelState.AddModelError("", "Invalid username or password.");
            return View(user);
        }

        // Display the registration page
        [HttpGet]
        [Route("Register")]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        // Handle registration form submission
        [HttpPost]
        [Route("Register")]
        [AllowAnonymous]
        public IActionResult Register(User user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            // Check if username already exists
            if (_users.Exists(u => u.Name == user.Name))
            {
                ModelState.AddModelError("", "Username already exists.");
                return View(user);
            }

            // Add new user to the simulated database
            user.Id = _users.Count > 0 ? _users[^1].Id + 1 : 1;
            _users.Add(user);

            TempData["Message"] = "Registration successful! Please log in.";
            return RedirectToAction("Login");
        }

        // Handle logout
        [HttpGet]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            // Sign out the user
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            TempData["Message"] = "You have been logged out.";
            return RedirectToAction("Index", "Home");
        }
    }
}
