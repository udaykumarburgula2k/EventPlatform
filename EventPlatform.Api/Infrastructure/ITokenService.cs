using EventPlatform.Api.Modules.Auth;

namespace EventPlatform.Api.Infrastructure;

public interface ITokenService
{
    Task<string> CreateTokenAsync(ApplicationUser user);
}