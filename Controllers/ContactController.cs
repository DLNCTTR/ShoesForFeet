using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ShoesForFeet.Controllers
{
    [Authorize]
    public class ContactController : Controller
    {
        public IActionResult Contact()
        {
            return View();
        }
    }
}