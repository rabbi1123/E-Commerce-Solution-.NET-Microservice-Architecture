using Notification.Application.Common;
using Notification.Application.Interfaces;

namespace Notification.Infrastructure.Email;

public sealed class EmailTemplateService : IEmailTemplateService
{
	public NotificationContent Render(string templateName, IReadOnlyDictionary<string, string> tokens)
	{
		return templateName switch
		{
			NotificationTemplateNames.EmailVerification => Content(
				"Verify your email address",
				$"Welcome.\nPlease verify your email address.\n{Token(tokens, "VerificationUrl")}"),
			_ => throw new InvalidOperationException($"Unknown notification template '{templateName}'.")
		};
	}

	private static NotificationContent Content(string heading, string body) => new(heading, heading, body);

	private static string Token(IReadOnlyDictionary<string, string> tokens, string name) =>
		tokens.TryGetValue(name, out var value) ? value : string.Empty;
}
