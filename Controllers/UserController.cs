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
        // Simulated database of users
        private static List<User> _users = new()
        {
            new User { Id = 1, Name = "JohnDoe", Password = "password123" },
            new User { Id = 2, Name = "JaneSmith", Password = "12345" }
        };

        // Displays the login form
        [HttpGet]
        [Route("Login")]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        // Handles login form submission
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
                    new Claim(ClaimTypes.Role, existingUser.Name == "Admin" ? "Admin" : "User")
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid username or password");
            return View(user);
        }

        // Logs out the current user
        [HttpGet]
        [Route("Logout")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        // Displays the form to add a new user
        [HttpGet]
        [Route("Add")]
        [Authorize(Roles = "Admin")] // Only Admin can add new users
        public IActionResult Add()
        {
            return View();
        }

        // Handles the form submission to add a new user
        [HttpPost]
        [Route("Add")]
        [Authorize(Roles = "Admin")] // Only Admin can add new users
        public IActionResult Add(User user)
        {
            if (ModelState.IsValid)
            {
                user.Id = _users.Count > 0 ? _users[^1].Id + 1 : 1; // Generate a unique ID
                _users.Add(user);
                return RedirectToAction("List");
            }

            return View(user);
        }

        // Displays the list of users
        [HttpGet]
        [Route("List")]
        [Authorize] // Only logged-in users can view the list
        public IActionResult List()
        {
            return View(_users);
        }

        // Displays the form to edit an existing user
        [HttpGet]
        [Route("Edit/{id}")]
        [Authorize(Roles = "Admin")] // Only Admin can edit users
        public IActionResult Edit(int id)
        {
            var user = _users.Find(u => u.Id == id);
            if (user == null)
            {
                return RedirectToAction("ErrorPage", "Error");
            }

            return View(user);
        }

        // Handles the form submission to edit an existing user
        [HttpPost]
        [Route("Edit")]
        [Authorize(Roles = "Admin")] // Only Admin can edit users
        public IActionResult Edit(User updatedUser)
        {
            var user = _users.Find(u => u.Id == updatedUser.Id);
            if (user == null)
            {
                return RedirectToAction("ErrorPage", "Error");
            }

            if (ModelState.IsValid)
            {
                user.Name = updatedUser.Name;
                user.Password = updatedUser.Password;
                return RedirectToAction("List");
            }

            return View(updatedUser);
        }

        // Displays the profile for a specific user
        [HttpGet]
        [Route("Profile/{id?}")]
        [Authorize] // Only logged-in users can view profiles
        public IActionResult Profile(int? id)
        {
            if (!id.HasValue || id == 0)
            {
                return RedirectToAction("ErrorPage", "Error");
            }

            var user = _users.Find(u => u.Id == id);
            if (user == null)
            {
                return RedirectToAction("ErrorPage", "Error");
            }

            return View(user);
        }

        // Displays the Contact Us page (restricted to logged-in users)
        [HttpGet]
        [Route("Contact")]
        [Authorize] // Only logged-in users can access the Contact Us page
        public IActionResult Contact()
        {
            return View();
        }
    }
}
