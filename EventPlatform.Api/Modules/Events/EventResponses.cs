namespace EventPlatform.Api.Modules.Events;

//public record TicketTypeResponse(
//    Guid Id,
//    Guid EventId,
//    string Name,
//    decimal Price,
//    int QuantityAvailable);

public record AgendaItemResponse(
    Guid Id,
    string Title,
    DateTime StartUtc,
    DateTime EndUtc);

public record SpeakerResponse(
    Guid Id,
    string Name,
    string Bio);

public record EventResponse(
    Guid Id,
    string Title,
    string Description,
    DateTime DateUtc,
    string Venue,
    Guid OrganizerId,
    List<AgendaItemResponse> AgendaItems,
    List<SpeakerResponse> Speakers,
    List<TicketTypeResponse> TicketTypes);


public class EventAccess
{
    public Guid EventId { get; set; }
    public Guid UserId { get; set; }
    public string Permission { get; set; } = "View";
}