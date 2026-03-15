using System.Net.Sockets;

namespace EventPlatform.Api.Modules.Events;

public class Event
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime DateUtc { get; set; }
    public string Venue { get; set; } = string.Empty;
    public Guid OrganizerId { get; set; }

    public List<AgendaItem> AgendaItems { get; set; } = [];
    public List<Speaker> Speakers { get; set; } = [];
    public List<TicketType> TicketTypes { get; set; } = [];
}