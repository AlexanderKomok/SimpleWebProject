using Microsoft.AspNetCore.Identity;

namespace WebAppTry3.Models
{
    public class User : IdentityUser
    {
        public int Year { get; set; }
    }
}
