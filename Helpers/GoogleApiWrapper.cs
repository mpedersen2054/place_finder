using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using PlaceFinder.Models;

namespace PlaceFinder.Helpers
{
    public class GoogleApiWrapper
    {
        private readonly IOptions<GoogleApiOptions> GoogleApisConfig;
        private string RootGeocodeUri = "https://maps.googleapis.com/maps/api/geocode/json?";
        private string RootPlacesUri = "https://maps.googleapis.com/maps/api/place/nearbysearch/json?";
        private string RootPlaceDetialsUri = "https://maps.googleapis.com/maps/api/place/details/json?";

        public GoogleApiWrapper(IOptions<GoogleApiOptions> config)
        {
            GoogleApisConfig = config;
        }
        // get the keys from appsettings.json
        private string PlacesKey {
            get { return GoogleApisConfig.Value.PlacesKey; }
        }
        private string GeocodingKey {
            get { return GoogleApisConfig.Value.GeocodingKey; }
        }

        // Receive string Address and returns [ lat, lng ]
        public async Task GetCoords(string Place, Action<float[]>Callback)
        {
            using (var Client = new HttpClient())
            {
                try 
                {
                    Client.BaseAddress = new Uri($"{RootGeocodeUri}address={Place}&key={GeocodingKey}");
                    HttpResponseMessage Response = await Client.GetAsync("");
                    Response.EnsureSuccessStatusCode(); // throw Exception if err
                    string StringResponse = await Response.Content.ReadAsStringAsync();
                    dynamic RObj = JsonConvert.DeserializeObject(StringResponse);
                    
                    // write error handling here...

                    float Lat = (float)RObj.results[0]["geometry"]["location"]["lat"];
                    float Lng = (float)RObj.results[0]["geometry"]["location"]["lng"];
                    Callback(new []{ Lat, Lng });
                }
                catch (HttpRequestException err)
                {
                    System.Console.WriteLine("Something we wrong");
                    System.Console.WriteLine(err);
                }
            }
        }

        // Recieve [ lat, lng ] & { Place: X, Service: Y, Keyword: Z } and returns { x: {}, x: {} }
        public async Task GetPlaces(float[] LatLng, Dictionary<string,string> Lookup, Action<Dictionary<string,object>>Callback)
        {
            string PlacesUrl = RootPlacesUri;
            PlacesUrl += $"location={LatLng[0]},{LatLng[1]}";
            PlacesUrl += $"&radius=350";
            PlacesUrl += $"&type={Lookup["Service"]}";
            // only add &keyword=... if it isnt null or blank
            if (Lookup["Keyword"] != null && Lookup["Keyword"].Length > 0)
            {
                PlacesUrl += $"&keyword={Lookup["Keyword"]}";
            }
            PlacesUrl += $"&key={PlacesKey}";

            using (var Client = new HttpClient())
            {
                try
                {
                    Client.BaseAddress = new Uri(PlacesUrl);
                    HttpResponseMessage Response = await Client.GetAsync("");
                    Response.EnsureSuccessStatusCode();
                    string StringResponse = await Response.Content.ReadAsStringAsync();
                    Dictionary<string,object> RObj = JsonConvert.DeserializeObject<Dictionary<string,object>>(StringResponse);

                    // write error handling here...

                    Callback(RObj);
                }
                catch (HttpRequestException err)
                {
                    System.Console.WriteLine("Something we wrong");
                    System.Console.WriteLine(err);
                }
            }
        }

        public async Task GetPlaceDetails(string PlaceId, Action<PlaceResults>Callback)
        {
            using (var Client = new HttpClient())
            {
                try
                {
                    Client.BaseAddress = new Uri($"{RootPlaceDetialsUri}placeid={PlaceId}&key={PlacesKey}");
                    HttpResponseMessage Response = await Client.GetAsync("");
                    Response.EnsureSuccessStatusCode();
                    string StringResponse = await Response.Content.ReadAsStringAsync();
                    Dictionary<string,object> RObj = JsonConvert.DeserializeObject<Dictionary<string,object>>(StringResponse);
                    PlaceResults Place = JsonConvert.DeserializeObject<PlaceResults>(RObj["result"].ToString());

                    // write error handling here...

                    Callback(Place);

                }
                catch(HttpRequestException err)
                {
                    System.Console.WriteLine("Something we wrong");
                    System.Console.WriteLine(err);
                }
            }
        }
    }
}