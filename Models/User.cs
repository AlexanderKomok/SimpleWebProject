using Microsoft.AspNetCore.Identity;
using WebAppTry3.DBEntities;
using System.Collections.Generic;

namespace WebAppTry3.Models
{
    public class User : IdentityUser
    {
        public int Year { get; set; }
        public string FullName { get; set; }

        public ICollection<Track> Tracks { get; set; }
        public ICollection<Album> Albums { get; set; }
    }
}
