namespace IdentityWebAPI.Settings;

public class IdentitySettings
{
    public const string SectionName = "Identity";

    public int EmailVerificationTokenMinutes { get; set; } = 30;
}
