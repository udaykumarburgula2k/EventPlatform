namespace EventPlatform.Api.Modules.Registration;

public record RegisterForEventRequest(
    Guid EventId,
    Guid TicketTypeId,
    string PaymentMethod,
    string PaymentToken);