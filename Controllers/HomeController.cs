using Microsoft.AspNetCore.Mvc;

namespace ShoesForFeet.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error(int statusCode)
        {
            if (statusCode == 404)
            {
                return View("Error");
            }
            return View("Error");
        }
    }
}