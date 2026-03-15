using Microsoft.AspNetCore.Identity;

namespace EventPlatform.Api.Modules.Auth;

public class ApplicationUser : IdentityUser<Guid>
{
    public string FullName { get; set; } = string.Empty;
}