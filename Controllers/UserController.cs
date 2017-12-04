using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Linq;
using PlaceFinder.Helpers;
using PlaceFinder.Models;
using PlaceFinder.Factory;

namespace PlaceFinder.Controllers
{
    public class UserController : Controller
    {
        private readonly UserFactory _userFactory;
        public UserController(IOptions<MySqlOptions> sqlOpts)
        {
            _userFactory = new UserFactory(sqlOpts);
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View("Index");
        }

        [HttpPost]
        [Route("login")]
        public IActionResult SubmitLogin(LoginViewModel user)
        {
            // validate data
            if (ModelState.IsValid)
            {
                User Person = _userFactory.FindOrCreate(user.name);
                HttpContext.Session.SetInt32("Id", Person._id);
                HttpContext.Session.SetString("Name", Person.name);
                return RedirectToAction("Index", "Map");
            }
            
            // validation failed
            return View("Index", user);
        }

        [HttpGet]
        [Route("users/{userId}")]
        public IActionResult ShowUser(int userId)
        {
            User _User = _userFactory.FindById(userId);
            System.Console.WriteLine(_User);
            System.Console.WriteLine(_User.name);
            System.Console.WriteLine(_User.places);
            System.Console.WriteLine(_User.places.Count);
            ViewBag.user = _User;
            return View("Show");
        }
    }
}
