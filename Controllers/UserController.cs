using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using PlaceFinder.Helpers;

namespace PlaceFinder.Controllers
{
    public class UserController : Controller
    {
        // [HttpPost]
        // [Route("login")]
        // public IActionResult SubmitLogin()
        // {
        //     // validate data
        //     // set stuff to session
        //     // redirect to /map
        //     return Redirect("Index", "Map");
        // }

        [HttpGet]
        [Route("users/{Uid}")]
        public IActionResult ShowUser(int Uid)
        {
            return View("Show");
        }
    }
}
