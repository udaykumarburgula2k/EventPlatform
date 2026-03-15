namespace EventPlatform.Api.Modules.Events;

public class TicketType
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid EventId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int QuantityAvailable { get; set; }

    public Event? Event { get; set; }
}