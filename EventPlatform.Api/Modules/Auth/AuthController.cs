using EventPlatform.Api.Common;
using EventPlatform.Api.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EventPlatform.Api.Modules.Auth;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ITokenService _tokenService;

    public AuthController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ITokenService tokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var allowedRoles = new[]
        {
            Roles.Organizer,
            Roles.Attendee
        };

        if (!allowedRoles.Contains(request.Role))
            return BadRequest("Only Organizer or Attendee registration is allowed.");

        var existing = await _userManager.FindByEmailAsync(request.Email);
        if (existing is not null)
            return BadRequest("Email already exists.");

        var user = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            FullName = request.FullName,
            Email = request.Email,
            UserName = request.Email,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        await _userManager.AddToRoleAsync(user, request.Role);

        var token = await _tokenService.CreateTokenAsync(user);
        return Ok(new AuthResponse(token, user.Email!, user.FullName, new[] { request.Role }));
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
            return Unauthorized("Invalid credentials.");

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!result.Succeeded)
            return Unauthorized("Invalid credentials.");

        var roles = await _userManager.GetRolesAsync(user);
        var token = await _tokenService.CreateTokenAsync(user);

        return Ok(new AuthResponse(token, user.Email!, user.FullName, roles));
    }
}