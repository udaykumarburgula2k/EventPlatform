using System.Security.Claims;
using EventPlatform.Api.Common;
using EventPlatform.Api.Data;
using Microsoft.AspNetCore.Authorization;

namespace EventPlatform.Api.Modules.Events.Authorization;

public class CanEditEventHandler : AuthorizationHandler<CanEditEventRequirement, Event>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        CanEditEventRequirement requirement,
        Event resource)
    {
        if (context.User.IsInRole(Roles.Administrator))
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                          ?? context.User.FindFirst("sub")?.Value;

        if (Guid.TryParse(userIdClaim, out var userId) &&
            context.User.IsInRole(Roles.Organizer) &&
            resource.OrganizerId == userId)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}