using System;
using System.ComponentModel.DataAnnotations;

namespace PlaceFinder.Models
{
    public class LoginViewModel : BaseEntity
    {
        [Required]
        [MinLength(3, ErrorMessage = "Name must be > 3 characters")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Name can only contain letters")]
        public string name { get; set; }
    }
}
