using System.Security.Claims;
using EventPlatform.Api.Common;
using EventPlatform.Api.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventPlatform.Api.Modules.Events;

[ApiController]
[Route("api/events")]
public class EventsController : ControllerBase
{
    private readonly AppDbContext _db;

    public EventsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var events = await _db.Events
            .Include(x => x.AgendaItems)
            .Include(x => x.Speakers)
            .Include(x => x.TicketTypes)
            .OrderBy(x => x.DateUtc)
            .ToListAsync();

        return Ok(events);
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(Guid id)
    {
        var evt = await _db.Events
            .Include(x => x.AgendaItems)
            .Include(x => x.Speakers)
            .Include(x => x.TicketTypes)
            .FirstOrDefaultAsync(x => x.Id == id);

        return evt is null ? NotFound() : Ok(evt);
    }

    [HttpPost]
    [Authorize(Roles = $"{Roles.Administrator},{Roles.Organizer}")]
    public async Task<IActionResult> Create(CreateEventRequest request)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue(ClaimTypes.Name)
            ?? User.FindFirstValue("sub")!);

        var evt = new Event
        {
            Title = request.Title,
            Description = request.Description,
            DateUtc = request.DateUtc,
            Venue = request.Venue,
            OrganizerId = userId
        };

        _db.Events.Add(evt);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = evt.Id }, evt);
    }

    [HttpPost("{eventId:guid}/agenda")]
    [Authorize(Roles = $"{Roles.Administrator},{Roles.Organizer}")]
    public async Task<IActionResult> AddAgenda(Guid eventId, AddAgendaItemRequest request)
    {
        var evt = await _db.Events.FindAsync(eventId);
        if (evt is null) return NotFound();

        var item = new AgendaItem
        {
            EventId = eventId,
            Title = request.Title,
            StartUtc = request.StartUtc,
            EndUtc = request.EndUtc
        };

        _db.AgendaItems.Add(item);
        await _db.SaveChangesAsync();

        return Ok(item);
    }

    [HttpPost("{eventId:guid}/speakers")]
    [Authorize(Roles = $"{Roles.Administrator},{Roles.Organizer}")]
    public async Task<IActionResult> AddSpeaker(Guid eventId, AddSpeakerRequest request)
    {
        var evt = await _db.Events.FindAsync(eventId);
        if (evt is null) return NotFound();

        var speaker = new Speaker
        {
            EventId = eventId,
            Name = request.Name,
            Bio = request.Bio
        };

        _db.Speakers.Add(speaker);
        await _db.SaveChangesAsync();

        return Ok(speaker);
    }

    [HttpPost("{eventId:guid}/tickets")]
    [Authorize(Roles = $"{Roles.Administrator},{Roles.Organizer}")]
    public async Task<IActionResult> AddTicketType(Guid eventId, AddTicketTypeRequest request)
    {
        var evt = await _db.Events.FindAsync(eventId);
        if (evt is null) return NotFound();

        var ticket = new TicketType
        {
            EventId = eventId,
            Name = request.Name,
            Price = request.Price,
            QuantityAvailable = request.QuantityAvailable
        };

        _db.TicketTypes.Add(ticket);
        await _db.SaveChangesAsync();

        return Ok(ticket);
    }
}