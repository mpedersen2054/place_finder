using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PlaceFinder.Models
{
    public class GeocodeResults : BaseEntity
    {
        public Dictionary<string,object>[] Results;
    }
}