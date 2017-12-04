using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace PlaceFinder.Models
{
    public class PlaceResults : BaseEntity
    {
        public int _id { get; set; }
        public string place_id { get; set; }
        public string name { get; set; }
        public string formatted_address { get; set; }
        public string formatted_phone_number { get; set; }
        public Dictionary<string,object> opening_hours { get; set; }
        public string[] types { get; set; }
        public PlaceResultsPhotos[] photos { get; set; }
    }

    public class PlaceResultsPhotos : BaseEntity
    {
        public string photo_reference { get; set; }
    }
}