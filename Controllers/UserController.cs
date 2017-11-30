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
                User Person = _userFactory.FindOrCreate(user.Name);
                HttpContext.Session.SetInt32("Id", Person.Id);
                HttpContext.Session.SetString("Name", Person.Name);
                return RedirectToAction("Index", "Map");
            }
            
            // validation failed
            return View("Index", user);
        }

        [HttpGet]
        [Route("users/{Uid}")]
        public IActionResult ShowUser(int Uid)
        {
            return View("Show");
        }
    }
}
