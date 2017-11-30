using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using PlaceFinder.Factory;

namespace PlaceFinder.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserFactory _userFactory;
        public HomeController(IOptions<MySqlOptions> sqlOpts)
        {
            _userFactory = new UserFactory(sqlOpts);
        }
        // GET: /Home/
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View("Index");
        }
    }
}
