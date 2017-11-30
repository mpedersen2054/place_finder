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

        public void sayHello()
        {
            System.Console.WriteLine("HELLO FROM GAW");
            System.Console.WriteLine("\nPLACES KEY");
            System.Console.WriteLine(PlacesKey);

            System.Console.WriteLine("\nGEOCODING KEY");
            System.Console.WriteLine(GeocodingKey);
        }
    }
}