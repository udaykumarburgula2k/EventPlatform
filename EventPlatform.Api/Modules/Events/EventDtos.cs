namespace EventPlatform.Api.Modules.Events;

public record CreateEventRequest(
    string Title,
    string Description,
    DateTime DateUtc,
    string Venue);

public record AddAgendaItemRequest(
    string Title,
    DateTime StartUtc,
    DateTime EndUtc);

public record AddSpeakerRequest(
    string Name,
    string Bio);

public record AddTicketTypeRequest(
    string Name,
    decimal Price,
    int QuantityAvailable);