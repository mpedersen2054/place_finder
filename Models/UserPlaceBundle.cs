using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PlaceFinder.Models
{
    public class UserPlaceBundle : BaseEntity
    {
        public User UserModel;
        public List<Place> PlaceModels;
    }
}