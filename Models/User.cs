using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PlaceFinder.Models
{
    public class User : BaseEntity
    {

        public User()
        {
            places = new List<Place>();
        }

        public int _id { get; set; }
        [Required]
        [MinLength(3)]
        public string name { get; set; }

        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }

        public ICollection<Place> places { get; set; }
        public int places_count { get; set; }
    }
}