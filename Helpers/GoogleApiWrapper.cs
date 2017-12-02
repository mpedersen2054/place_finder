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
            System.Console.WriteLine(GoogleApisConfig);
        }
        // get the keys from appsettings.json
        private string PlacesKey {
            get { return GoogleApisConfig.Value.PlacesKey; }
        }
        private string GeocodingKey {
            get { return GoogleApisConfig.Value.GeocodingKey; }
        }

        // Recieve Place, Service, Name > returns ...
        public async Task Lookup(LookupViewModel Lookup, Action<Dictionary<string,object>>Callback)
        {
            Dictionary<string,object> Places = new Dictionary<string,object>();
            // get geo-codes from location
            GetCoords(Lookup.Place, GResults => {
                GetPlaces(GResults, Lookup, PResults => {
                    Places = PResults;
                }).Wait();
            }).Wait();

            Callback(Places);
            // get places using the geo coords
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
        public async Task GetPlaces(float[] LatLng, LookupViewModel Lookup, Action<Dictionary<string,object>>Callback)
        {
            string PlacesUrl = RootPlacesUri;
            PlacesUrl += $"location={LatLng[0]},{LatLng[1]}";
            PlacesUrl += $"&radius=500";
            PlacesUrl += $"&type={Lookup.Service}";
            PlacesUrl += $"&keyword={Lookup.Keyword}";
            PlacesUrl += $"&key={PlacesKey}";
            
            System.Console.WriteLine(PlacesUrl);

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

        public async Task GetPlaceDetails(string PlaceId, Action<Dictionary<string,object>>Callback)
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
                    
                    // write error handling here...

                    Callback(RObj);

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