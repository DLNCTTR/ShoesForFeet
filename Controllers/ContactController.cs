using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

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