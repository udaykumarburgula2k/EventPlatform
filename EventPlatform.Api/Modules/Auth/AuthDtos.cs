namespace EventPlatform.Api.Modules.Auth;

public record RegisterRequest(
    string FullName,
    string Email,
    string Password,
    string Role);

public record LoginRequest(
    string Email,
    string Password);

public record AuthResponse(
    string Token,
    string Email,
    string FullName,
    IEnumerable<string> Roles);