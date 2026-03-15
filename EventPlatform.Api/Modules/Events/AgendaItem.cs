namespace EventPlatform.Api.Modules.Events;

public class AgendaItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid EventId { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime StartUtc { get; set; }
    public DateTime EndUtc { get; set; }

    public Event? Event { get; set; }
}