using EventPlatform.Api.Common;
using EventPlatform.Api.Modules.Auth;
using Microsoft.AspNetCore.Identity;

namespace EventPlatform.Api.Data;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider services)
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

        var roles = new[]
        {
            Roles.Administrator,
            Roles.Organizer,
            Roles.Attendee
        };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>(role));
            }
        }

        const string adminEmail = "admin@eventplatform.local";

        var admin = await userManager.FindByEmailAsync(adminEmail);
        if (admin is null)
        {
            admin = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                FullName = "System Admin",
                Email = adminEmail,
                UserName = adminEmail,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(admin, "Admin@12345");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, Roles.Administrator);
            }
        }
    }
}