using Microsoft.AspNetCore.Mvc;

namespace ShoesForFeet.Controllers
{
    [Route("User")]
    public class UserController : Controller
    {
        [Route("Profile/{id?}")] // Attribute route
        public IActionResult Profile(int? id)
        {
            ViewBag.UserId = id ?? 0;
            return View();
        }
    }
}