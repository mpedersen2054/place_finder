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
        private UserFactory _userFactory;
        public PlaceController(IOptions<GoogleApiOptions> opts, IOptions<MySqlOptions> sqlOpts)
        {
            _googleApiWrapper = new GoogleApiWrapper(opts);
            _placeFactory = new PlaceFactory(sqlOpts);
            _userFactory = new UserFactory(sqlOpts);
        }

        [HttpGet]
        [Route("place/{PlaceId}")]
        public JsonResult GetPlaceDetials(string PlaceId)
        {
            int? UserId = HttpContext.Session.GetInt32("Id");
            PlaceResults _Place = new PlaceResults();
            _googleApiWrapper.GetPlaceDetails(PlaceId, Results => {
                _Place = Results;
            }).Wait();

            System.Console.WriteLine(_Place);
            // get list of users' places' place_ids to send to frontend
            // to not allow user to add it again if place already added
            var placeIds = _userFactory.GetUsersPlaceIds((int)UserId);

            return Json(new { Place = _Place, UserPlaceIds = placeIds });
        }

        [HttpPost]
        [Route("place/{PlaceId}/add")]
        public JsonResult AddPlace(string PlaceId)
        {
            System.Console.WriteLine($"Adding place {PlaceId}");
            int? UserId = HttpContext.Session.GetInt32("Id");
            PlaceResults _Place = new PlaceResults();

            _googleApiWrapper.GetPlaceDetails(PlaceId, Results => {
                _Place = Results;
            }).Wait();

            _placeFactory.CreatePlace(Convert.ToInt32(UserId), _Place);

            // get users added places. Check if the place is already in user.places

            return Json(new {});
        }
    }
}
