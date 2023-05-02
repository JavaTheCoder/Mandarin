using Microsoft.AspNetCore.Identity;

namespace Mandarin.Web.Models
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
