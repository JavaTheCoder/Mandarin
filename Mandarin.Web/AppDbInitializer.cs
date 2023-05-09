using Mandarin.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace Mandarin.Web
{
    public class AppDbInitializer
    {
        public static async Task SeedRolesToDb(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRole.Admin.ToString()))
                {
                    await roleManager.CreateAsync(new IdentityRole(UserRole.Admin.ToString()));
                }

                if (!await roleManager.RoleExistsAsync(UserRole.Manager.ToString()))
                {
                    await roleManager.CreateAsync(new IdentityRole(UserRole.Manager.ToString()));
                }

                if (!await roleManager.RoleExistsAsync(UserRole.User.ToString()))
                {
                    await roleManager.CreateAsync(new IdentityRole(UserRole.User.ToString()));
                }
            }
        }
    }
}
