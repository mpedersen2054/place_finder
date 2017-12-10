using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PlaceFinder.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Default()
        {
            return View("~/View/Shared/Errors/Default.cshtml");
        }
    }
}