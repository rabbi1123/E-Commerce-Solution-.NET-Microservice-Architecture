using Eventbus.Messages.Events;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Options;
using Notification.Application.Common;
using Notification.Application.Features.Notifications.Commands;
using Notification.Infrastructure.Email;

namespace Notification.WebAPI.EventBusConsumer;

public class UserVerificationEmailConsumer : IConsumer<UserVerificationEmailEvent>
{
	private readonly IMediator _mediator;
	private readonly PublicWebSettings _settings;

	public UserVerificationEmailConsumer(IMediator mediator, IOptions<PublicWebSettings> settings)
	{
		_mediator = mediator;
		_settings = settings.Value;
	}

	public Task Consume(ConsumeContext<UserVerificationEmailEvent> context)
	{
		var message = context.Message;
		var baseUrl = _settings.PublicWebBaseUrl.TrimEnd('/');
		var verificationUrl =
			$"{baseUrl}/Account/verify-email?token={Uri.EscapeDataString(message.VerificationToken)}";

		return _mediator.Send(
			new SendEmailNotificationCommand
			{
				EventId = message.Id,
				TemplateName = NotificationTemplateNames.EmailVerification,
				Recipient = message.Email,
				Tokens = new Dictionary<string, string>
				{
					["VerificationUrl"] = verificationUrl
				}
			},
			context.CancellationToken);
	}
}
