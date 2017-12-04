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
        public JsonResult Lookup(LookupViewModel LUVMInfo)
        {
            // check is valid...

            Dictionary<string,string> Lookup = new Dictionary<string,string>();
            Lookup.Add("Place", LUVMInfo.Place);
            Lookup.Add("Service", LUVMInfo.Service);
            // check if keyword is there
            Lookup.Add("Keyword", LUVMInfo.Keyword);

            Dictionary<string,object> PlacesJson = new Dictionary<string,object>();

            _googleApiWrapper.Lookup(Lookup, Results => {
                PlacesJson = Results;
            }).Wait();
            return Json(PlacesJson);
        }

        [HttpGet]
        [Route("map/get_new_places/{Lat}/{Lng}/{Place}/{Service}/{Keyword}")]
        public JsonResult GetNewPlaces(float Lat, float Lng, string Place, string Service, string Keyword)
        {
            Dictionary<string,string> Lookup = new Dictionary<string,string>();
            Lookup.Add("Place", Place);
            Lookup.Add("Service", Service);
            // check if keyword is there
            Lookup.Add("Keyword", Keyword);
            Dictionary<string,object> PlacesJson = new Dictionary<string,object>();
            _googleApiWrapper.GetPlaces(new []{ Lat, Lng }, Lookup, Results => {
                PlacesJson = Results;
            }).Wait();
            return Json(PlacesJson);
        }

        [HttpGet]
        [Route("map/place/{PlaceId}")]
        public JsonResult GetPlaceDetials(string PlaceId)
        {
            Dictionary<string,object> PlaceDetailsJson = new Dictionary<string,object>();
            _googleApiWrapper.GetPlaceDetails(PlaceId, Results => {
                PlaceDetailsJson = Results;
            }).Wait();

            // get users added places. Check if the place is already in user.places

            return Json(PlaceDetailsJson);
        }
    }
}
