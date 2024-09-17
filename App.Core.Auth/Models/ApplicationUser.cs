using Microsoft.AspNetCore.Identity;

namespace App.Core.Auth.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
