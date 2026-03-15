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

    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var events = await _db.Events
            .Include(x => x.AgendaItems)
            .Include(x => x.Speakers)
            .Include(x => x.TicketTypes)
            .OrderBy(x => x.DateUtc)
            .ToListAsync();

        var response = events.Select(evt => new EventResponse(
            evt.Id,
            evt.Title,
            evt.Description,
            evt.DateUtc,
            evt.Venue,
            evt.OrganizerId,
            evt.AgendaItems.Select(a => new AgendaItemResponse(
                a.Id,
                a.Title,
                a.StartUtc,
                a.EndUtc)).ToList(),
            evt.Speakers.Select(s => new SpeakerResponse(
                s.Id,
                s.Name,
                s.Bio)).ToList(),
            evt.TicketTypes.Select(t => new TicketTypeResponse(
                t.Id,
                t.EventId,
                t.Name,
                t.Price,
                t.QuantityAvailable)).ToList()
        )).ToList();

        return Ok(response);
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

        if (evt is null)
            return NotFound();

        var response = new EventResponse(
            evt.Id,
            evt.Title,
            evt.Description,
            evt.DateUtc,
            evt.Venue,
            evt.OrganizerId,
            evt.AgendaItems.Select(a => new AgendaItemResponse(
                a.Id,
                a.Title,
                a.StartUtc,
                a.EndUtc)).ToList(),
            evt.Speakers.Select(s => new SpeakerResponse(
                s.Id,
                s.Name,
                s.Bio)).ToList(),
            evt.TicketTypes.Select(t => new TicketTypeResponse(
                t.Id,
                t.EventId,
                t.Name,
                t.Price,
                t.QuantityAvailable)).ToList()
        );

        return Ok(response);
    }


    // [Authorize(Roles = $"{Roles.Administrator},{Roles.Organizer}")]

    [HttpPost]
    [Authorize(Policy = Policies.ManageEvents)]
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

        var response = new EventResponse(
            evt.Id,
            evt.Title,
            evt.Description,
            evt.DateUtc,
            evt.Venue,
            evt.OrganizerId,
            new List<AgendaItemResponse>(),
            new List<SpeakerResponse>(),
            new List<TicketTypeResponse>());

       return CreatedAtAction(nameof(GetById), new { id = evt.Id }, response);
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

        var response = new TicketTypeResponse(
            ticket.Id,
            ticket.EventId,
            ticket.Name,
            ticket.Price,
            ticket.QuantityAvailable);

        return Ok(response);
    }
}