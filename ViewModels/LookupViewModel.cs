using System;
using System.ComponentModel.DataAnnotations;

namespace PlaceFinder.Models
{
    public class LookupViewModel : BaseEntity
    {
        [Required]
        [MinLength(3, ErrorMessage = "Place must be > 3 characters")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Name can only contain letters")]
        public string Place { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Service must be > 3 characters")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Name can only contain letters")]
        public string Service { get; set; }

        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Name can only contain letters")]
        public string Keyword { get; set; }
    }
}
