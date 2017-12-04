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
        public bool is_open { get; set; }
        public PlaceHours[] hours { get; set; }
        public PlaceTypes[] types { get; set; }
        public PlacePhotos[] photos { get; set; }

        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }

        public int users__id { get; set; }
    }

    public class PlaceHours : BaseEntity
    {
        public int _id { get; set; }
        public string text { get; set; }
        public int order_pos { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public int places__id { get; set; }
    }

    public class PlaceTypes : BaseEntity
    {
        public int _id { get; set; }
        public string text { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public int places__id { get; set; }
    }

    public class PlacePhotos : BaseEntity
    {
        public int _id { get; set; }
        public string reference { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public int places__id { get; set; }
    }
}