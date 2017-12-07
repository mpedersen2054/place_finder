using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PlaceFinder.Models
{
    public class UsersPlaces : BaseEntity
    {
        public int users__id { get; set; }
        public int places__id { get; set; }
    }
}