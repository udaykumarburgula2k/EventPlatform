namespace EventPlatform.Api.Modules.Auth;

public class AuthModuleOptions
{
    public const string SectionName = "Modules:Auth";

    public bool AllowOrganizerSelfRegistration { get; set; } = true;
    public bool AllowAttendeeSelfRegistration { get; set; } = true;
    public bool RequireEmailVerification { get; set; } = false;
}