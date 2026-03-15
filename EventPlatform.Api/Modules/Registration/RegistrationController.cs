using EventPlatform.Api.Common;
using EventPlatform.Api.Data;
using EventPlatform.Api.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace EventPlatform.Api.Modules.Registration;

[ApiController]
[Route("api/registrations")]
public class RegistrationsController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IPaymentService _paymentService;

    private readonly RegistrationModuleOptions _options;
    public RegistrationsController(AppDbContext db, IPaymentService paymentService, IOptions<RegistrationModuleOptions> options)
    {
        _db = db;
        _paymentService = paymentService;
        _options = options.Value;
    }

    [HttpPost]
    [Authorize(Roles = $"{Roles.Administrator},{Roles.Attendee}")]
    public async Task<IActionResult> Register(RegisterForEventRequest request)
    {
        if (!_options.Enabled)
            return BadRequest("Registration module is disabled.");

        var userIdClaim =
            User.FindFirstValue(ClaimTypes.NameIdentifier) ??
            User.FindFirstValue("sub");

        if (string.IsNullOrWhiteSpace(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized("User id claim is missing or invalid.");
        }

        var evt = await _db.Events.FindAsync(request.EventId);
        if (evt is null) return NotFound("Event not found.");

        var ticket = await _db.TicketTypes
            .FirstOrDefaultAsync(x => x.Id == request.TicketTypeId && x.EventId == request.EventId);

        if (ticket is null) return NotFound("Ticket type not found.");
        if (ticket.QuantityAvailable <= 0) return BadRequest("Ticket sold out.");

        var payment = await _paymentService.ChargeAsync(
            ticket.Price,
            request.PaymentMethod,
            request.PaymentToken);

        if (!payment.Success)
            return BadRequest("Payment failed.");

        ticket.QuantityAvailable--;

        var registration = new Registration
        {
            EventId = request.EventId,
            TicketTypeId = request.TicketTypeId,
            UserId = userId,
            PaymentStatus = "Paid",
            PaymentReference = payment.Reference
        };

        _db.Registrations.Add(registration);
        await _db.SaveChangesAsync();

        return Ok(registration);
    }

    [HttpGet("my")]
    [Authorize]
    public async Task<IActionResult> MyRegistrations()
    {
        //var userId = Guid.Parse(User.FindFirstValue("sub")!);
        var userIdClaim =
            User.FindFirstValue(ClaimTypes.NameIdentifier) ??
            User.FindFirstValue("sub");

        if (string.IsNullOrWhiteSpace(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized("User id claim is missing or invalid.");
        }

        var registrations = await _db.Registrations
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.RegisteredAtUtc)
            .ToListAsync();

        return Ok(registrations);
    }
}