using System;
using System.Collections.Generic;
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
        public Dictionary<string,object> opening_hours { get; set; }
        public string[] types { get; set; }
        public Dictionary<string,object>[] photos { get; set; }
    }
}