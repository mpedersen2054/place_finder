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
    public class PlaceController : Controller
    {
        private GoogleApiWrapper _googleApiWrapper;
        public PlaceController(IOptions<GoogleApiOptions> opts)
        {
            _googleApiWrapper = new GoogleApiWrapper(opts);
        }

        [HttpGet]
        [Route("place/{PlaceId}")]
        public JsonResult GetPlaceDetials(string PlaceId)
        {
            Place _Place = new Place();
            _googleApiWrapper.GetPlaceDetails(PlaceId, Results => {
                _Place = Results;
            }).Wait();

            System.Console.WriteLine(_Place);
            // get users added places. Check if the place is already in user.places

            return Json(_Place);
        }

        [HttpPost]
        [Route("place/{PlaceId}/add")]
        public JsonResult AddPlace(string PlaceId)
        {
            System.Console.WriteLine($"Adding place {PlaceId}");

            // get users added places. Check if the place is already in user.places

            return Json(new {});
        }
    }
}
