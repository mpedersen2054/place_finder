using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using PlaceFinder.Helpers;
using PlaceFinder.Models;
using PlaceFinder.Factory;

namespace PlaceFinder.Controllers
{
    public class PlaceController : Controller
    {
        private GoogleApiWrapper _googleApiWrapper;
        private PlaceFactory _placeFactory;
        public PlaceController(IOptions<GoogleApiOptions> opts, IOptions<MySqlOptions> sqlOpts)
        {
            _googleApiWrapper = new GoogleApiWrapper(opts);
            _placeFactory = new PlaceFactory(sqlOpts);
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
            int? UserId = HttpContext.Session.GetInt32("Id");
            Place _Place = new Place();

            _googleApiWrapper.GetPlaceDetails(PlaceId, Results => {
                _Place = Results;
            }).Wait();

            _placeFactory.CreatePlace(Convert.ToInt32(UserId), _Place);

            // get users added places. Check if the place is already in user.places

            return Json(new {});
        }
    }
}
