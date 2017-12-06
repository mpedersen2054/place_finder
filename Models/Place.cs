using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace PlaceFinder.Models
{
    public class Place : BaseEntity
    {

        public Place()
        {
            hours = new List<PlaceHours>();
            types = new List<PlaceTypes>();
            photos = new List<PlacePhotos>();
            reviews = new List<Review>();
        }

        public int _id { get; set; }
        public string place_id { get; set; }
        public string name { get; set; }
        public string formatted_address { get; set; }
        public string formatted_phone_number { get; set; }
        public bool is_open { get; set; }
        public float rating { get; set; }
        public ICollection<PlaceHours> hours { get; set; }
        public ICollection<PlaceTypes> types { get; set; }
        public ICollection<PlacePhotos> photos { get; set; }
        public ICollection<Review> reviews { get; set; }

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