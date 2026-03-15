namespace EventPlatform.Api.Modules.Registration;

public class Registration
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid EventId { get; set; }
    public Guid TicketTypeId { get; set; }
    public Guid UserId { get; set; }

    public DateTime RegisteredAtUtc { get; set; } = DateTime.UtcNow;
    public string PaymentStatus { get; set; } = "Pending";
    public string PaymentReference { get; set; } = string.Empty;
}