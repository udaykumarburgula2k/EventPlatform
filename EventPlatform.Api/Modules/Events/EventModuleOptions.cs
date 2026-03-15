namespace EventPlatform.Api.Modules.Events;

public class EventModuleOptions
{
    public const string SectionName = "Modules:Events";

    public bool Enabled { get; set; } = true;
    public bool AllowDelete { get; set; } = false;
    public bool RequireApprovalBeforePublish { get; set; } = true;
    public int MaxSpeakersPerEvent { get; set; } = 10;
}