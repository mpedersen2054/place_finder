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
        public IActionResult Home()
        {
            return View("Home");
        }

        [HttpPost]
        [Route("login")]
        public IActionResult SubmitLogin(LoginViewModel user)
        {
            // validate data
            if (ModelState.IsValid)
            {
                User _User = _userFactory.FindOrCreate(user.name);
                HttpContext.Session.SetInt32("Id", _User._id);
                HttpContext.Session.SetString("Name", _User.name);
                return RedirectToAction("Index", "Map");
            }
            
            // validation failed
            return View("Home", user);
        }

        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {   
            HttpContext.Session.Clear();
            return RedirectToAction("Home");
        }

        [HttpGet]
        [Route("users")]
        public IActionResult UserList()
        {
            int? UserId = HttpContext.Session.GetInt32("Id");
            if (UserId == null)
            {
                return RedirectToAction("Home");
            }
            ViewBag.userId = UserId;
            
            return View("Index");
        }

        [HttpGet]
        [Route("users/{userId}")]
        public IActionResult ShowUser(int userId)
        {
            int? UserId = HttpContext.Session.GetInt32("Id");
            if (UserId == null)
            {
                return RedirectToAction("Home");
            }
            User _User = _userFactory.FindById(userId);
            ViewBag.user = _User;
            ViewBag.userId = UserId;
            
            return View("Show");
        }
    }
}
