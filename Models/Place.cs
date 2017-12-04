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
        public string[] hours { get; set; }
        public string[] types { get; set; }
        public string[] photos { get; set; }

        public int users__id { get; set; }
    }
}