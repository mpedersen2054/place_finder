using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using PlaceFinder.Helpers;
using PlaceFinder.Models;

namespace PlaceFinder.Controllers
{
    public class MapController : Controller
    {
        private GoogleApiWrapper _googleApiWrapper;
        public MapController(IOptions<GoogleApiOptions> opts)
        {
            _googleApiWrapper = new GoogleApiWrapper(opts);
        }

        [HttpGet]
        [Route("map")]
        public IActionResult Index()
        {
            int? UserId = HttpContext.Session.GetInt32("Id");
            if (UserId == null)
            {
                return RedirectToAction("Index", "User");
            }
            return View("Index");
        }

        [HttpPost]
        [Route("map/lookup")]
        public JsonResult Lookup(LookupViewModel LookupInfo)
        {
            Dictionary<string,object> PlacesJson = new Dictionary<string,object>();
            _googleApiWrapper.Lookup(LookupInfo, Results => {
                PlacesJson = Results;
            }).Wait();
            return Json(PlacesJson);
        }

        [HttpGet]
        [Route("map/place/{PlaceId}")]
        public JsonResult GetPlaceDetials(string PlaceId)
        {
            Dictionary<string,object> PlaceDetailsJson = new Dictionary<string,object>();
            _googleApiWrapper.GetPlaceDetails("ChIJN1t_tDeuEmsRUsoyG83frY4", Results => {
                PlaceDetailsJson = Results;
            }).Wait();
            return Json(PlaceDetailsJson);
        }
    }
}
