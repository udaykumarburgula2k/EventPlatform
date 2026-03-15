namespace EventPlatform.Api.Modules.Registration;

public class RegistrationModuleOptions
{
    public const string SectionName = "Modules:Registration";

    public bool Enabled { get; set; } = true;
    public bool PaymentRequired { get; set; } = true;
    public int MaxTicketsPerUserPerEvent { get; set; } = 5;
    public int CancellationWindowHours { get; set; } = 24;
}