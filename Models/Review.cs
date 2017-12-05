using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PlaceFinder.Models
{
    public class Review : BaseEntity
    {
        public int _id { get; set; }
        public string text { get; set; }

        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }

        public int users__id { get; set; }
        public int places__id { get; set; }
    }
}