using System;
using System.Collections.Generic;
using System.Text;

namespace AppProfile.Models
{
    public class UserProfile
    {
        public required string Name { get; set; }
        public required int Age { get; set; }
        public string? Description { get; set; }
        public string? ProfileImageUrl { get; set; }
    }
}
