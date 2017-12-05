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
                return RedirectToAction("Home", "User");
            }
            
            ViewBag.userId = UserId;
            return View("Index");
        }

        [HttpPost]
        [Route("map/lookup")]
        public JsonResult Lookup(LookupViewModel LUVMInfo)
        {
            // check is valid here...
            Dictionary<string,object> PlacesJson = new Dictionary<string,object>();
            Dictionary<string,string> Lookup = new Dictionary<string,string>();
            Lookup.Add("Place", LUVMInfo.Place);
            Lookup.Add("Service", LUVMInfo.Service);
            // check if keyword is there
            Lookup.Add("Keyword", LUVMInfo.Keyword);

            // query for coords, then using the coords query for places
            _googleApiWrapper.GetCoords(Lookup["Place"], GResults => {
                _googleApiWrapper.GetPlaces(GResults, Lookup, PResults => {
                    PResults["Coords"] = GResults;
                    PlacesJson = PResults;
                }).Wait();
            }).Wait();

            return Json(PlacesJson);
        }

        [HttpGet]
        [Route("map/get_new_places/{Lat}/{Lng}/{Place}/{Service}/{Keyword}")]
        public JsonResult GetNewPlaces(float Lat, float Lng, string Place, string Service, string Keyword)
        {
            Dictionary<string,object> PlacesJson = new Dictionary<string,object>();
            Dictionary<string,string> Lookup = new Dictionary<string,string>();
            Lookup.Add("Place", Place);
            Lookup.Add("Service", Service);
            // check if keyword is there
            Lookup.Add("Keyword", Keyword);

            _googleApiWrapper.GetPlaces(new []{ Lat, Lng }, Lookup, Results => {
                PlacesJson = Results;
            }).Wait();

            return Json(PlacesJson);
        }
    }
}
