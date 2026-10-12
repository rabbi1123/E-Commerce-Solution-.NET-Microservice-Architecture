using Notification.Application.Common;
using Notification.Application.Interfaces;
using Notification.Domain.Entities;

namespace Notification.Application.Features.Notifications.Commands;

internal static class SendNotificationHandler
{
	public static async Task<CommandResponse> HandleAsync(
		INotificationRepository repository,
		IEmailTemplateService templates,
		Guid eventId,
		string channel,
		string recipient,
		string templateName,
		IReadOnlyDictionary<string, string> tokens,
		Func<NotificationContent, (string? Title, string? Subject, string Body)> select,
		Func<NotificationContent, CancellationToken, Task<DeliveryResult>> send,
		CancellationToken cancellationToken)
	{
		if (await repository.ExistsAsync(eventId, channel, cancellationToken))
			return new CommandResponse { IsSuccess = true };

		var content = templates.Render(templateName, tokens);
		var (title, subject, body) = select(content);
		var delivery = await send(content, cancellationToken);

		var id = await repository.AddAsync(
			NotificationMessage.Sent(
				eventId,
				channel,
				recipient,
				templateName,
				title,
				subject,
				body,
				delivery.Provider,
				delivery.ProviderMessageId),
			cancellationToken);

		return new CommandResponse { IsSuccess = true, Id = id };
	}
}
