using System;
using System.ComponentModel.DataAnnotations;

namespace PlaceFinder.Models
{
    public class User : BaseEntity
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3)]
        public string Name { get; set; }
    }
}