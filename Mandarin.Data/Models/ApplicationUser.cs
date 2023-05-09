using Microsoft.AspNetCore.Identity;

namespace Mandarin.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }

    public enum UserRole
    {
        Admin,
        Manager,
        User
    }
}
