using Microsoft.AspNetCore.Mvc;

namespace ShoesForFeet.Controllers
{
    public class ErrorController : Controller
    {
        [HttpGet]
        [Route("Error/Error404")]
        public IActionResult Error404()
        {
            return View(); // Returns the custom 404 error view
        }
    }
}