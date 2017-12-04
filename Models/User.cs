using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PlaceFinder.Models
{
    public class User : BaseEntity
    {
        public int _id { get; set; }
        [Required]
        [MinLength(3)]
        public string name { get; set; }

        public List<Place> places { get; set; }
    }
}