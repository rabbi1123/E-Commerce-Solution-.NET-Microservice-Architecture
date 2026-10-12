namespace Notification.Infrastructure.Email;

public sealed class PublicWebSettings
{
	public const string SectionName = "Notifications";

	public string PublicWebBaseUrl { get; set; } = "http://localhost:8086";
}
