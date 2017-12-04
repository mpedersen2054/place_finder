using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace PlaceFinder.Models
{
    public class PlaceResults : BaseEntity
    {
        public string place_id { get; set; }
        public string name { get; set; }
        public string formatted_address { get; set; }
        public string formatted_phone_number { get; set; }
        public PlaceResultsOpeningHours opening_hours { get; set; }
        public string[] types { get; set; }
        public PlaceResultsPhotos[] photos { get; set; }
    }

    public class PlaceResultsOpeningHours : BaseEntity
    {
        public bool is_open { get; set; }
        public string[] weekday_text { get; set; }
    }

    public class PlaceResultsPhotos : BaseEntity
    {
        public string photo_reference { get; set; }
    }
}