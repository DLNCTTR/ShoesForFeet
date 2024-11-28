using Microsoft.AspNetCore.Mvc;
using ShoesForFeet.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ShoesForFeet.Controllers
{
    [Route("User")]
    public class UserController : Controller
    {
        // Simulating a database with a static list of users
        private static List<User> _users = new()
        {
            new User { Id = 1, Name = "JohnDoe", Password = "password123" },
            new User { Id = 2, Name = "JaneSmith", Password = "12345" }
        };

        // Displays the form to add a new user (default landing page for "User")
        [HttpGet]
        [Route("Add")]
        public IActionResult Add()
        {
            return View(); // Return the Add User form view
        }

        // Handles the submission of the form to add a new user
        [HttpPost]
        [Route("Add")]
        public IActionResult Add(User user)
        {
            if (ModelState.IsValid) // Validate user input
            {
                user.Id = _users.Count > 0 ? _users[^1].Id + 1 : 1; // Assign unique ID
                _users.Add(user); // Add user to the list
                return RedirectToAction("List"); // Redirect to the user list
            }

            return View(user); // Re-display the form if validation fails
        }

        // Displays the form to log in
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
                var authProperties = new AuthenticationProperties();
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "Invalid username or password");
            return View(user);
        }

        // Logs the user out
        [HttpGet]
        [Route("Logout")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        // Displays a list of all users
        [HttpGet]
        [Route("List")]
        public IActionResult List()
        {
            return View(_users); // Return the list of users to the view
        }

        // Displays the form to edit an existing user
        [HttpGet]
        [Route("Edit/{id}")]
        public IActionResult Edit(int id)
        {
            var user = _users.Find(u => u.Id == id); // Find user by ID
            if (user == null)
            {
                // Redirect to a custom 404 error page if user not found
                return RedirectToAction("ErrorPage", "Error");
            }

            return View(user); // Pass the user to the edit form view
        }

        // Handles the submission of the form to update user details
        [HttpPost]
        [Route("Edit")]
        public IActionResult Edit(User updatedUser)
        {
            var user = _users.Find(u => u.Id == updatedUser.Id); // Find the existing user
            if (user == null)
            {
                // Redirect to a custom error page if user not found
                return RedirectToAction("ErrorPage", "Error");
            }

            if (ModelState.IsValid)
            {
                // Update user properties
                user.Name = updatedUser.Name;
                user.Password = updatedUser.Password;
                return RedirectToAction("List"); // Redirect to the user list after successful update
            }

            return View(updatedUser); // Re-display the form if validation fails
        }

        // Displays the profile for a specific user
        [HttpGet]
        [Route("Profile/{id?}")]
        public IActionResult Profile(int? id)
        {
            if (!id.HasValue || id == 0)
            {
                // Redirect to a custom 404 error page if no user ID provided
                return RedirectToAction("ErrorPage", "Error");
            }

            var user = _users.Find(u => u.Id == id); // Find user by ID
            if (user == null)
            {
                // Redirect to a custom 404 error page if user not found
                return RedirectToAction("ErrorPage", "Error");
            }

            return View(user); // Pass the user to the profile view
        }
    }
}
