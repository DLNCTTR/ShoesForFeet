using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using ShoesForFeet.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace ShoesForFeet.Controllers
{
    [Route("User")]
    public class UserController : Controller
    {
        private static List<User> _users = new()
        {
            new User { Id = 1, Name = "Admin", Password = "admin123", Role = "Admin" },
            new User { Id = 2, Name = "User1", Password = "user123", Role = "User" }
        };

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
        public IActionResult Login(User user)
        {
            var existingUser = _users.Find(u => u.Name == user.Name && u.Password == user.Password);
            if (existingUser != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, existingUser.Name),
                    new Claim(ClaimTypes.Role, existingUser.Role)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

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
                user.Id = _users.Count > 0 ? _users[^1].Id + 1 : 1;
                _users.Add(user);
                TempData["Message"] = "Registration successful! Please log in.";
                return RedirectToAction("Login");
            }

            return View(user);
        }
    }
}
