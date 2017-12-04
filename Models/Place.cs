using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace PlaceFinder.Models
{
    public class Place : BaseEntity
    {
        public int _id { get; set; }
        public string place_id { get; set; }
        public string name { get; set; }
        public string formatted_address { get; set; }
        public string formatted_phone_number { get; set; }
        public PlaceOpeningHours opening_hours { get; set; }
        public string[] types { get; set; }
        public PlacePhotos[] photos { get; set; }

        public int user_id { get; set; }
    }

    public class PlaceOpeningHours : BaseEntity
    {
        public bool is_open { get; set; }
        public string[] weekday_text { get; set; }
    }

    public class PlacePhotos : BaseEntity
    {
        public string photo_reference { get; set; }
    }
}