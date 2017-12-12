using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PlaceFinder
{
    public class ErrorController : Controller
    {
        public IActionResult Default()
        {
            return View("~/View/Shared/Errors/Default.cshtml");
        }
    }
}