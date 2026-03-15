namespace EventPlatform.Api.Modules.Events;

public class Speaker
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid EventId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;

    public Event? Event { get; set; }
}