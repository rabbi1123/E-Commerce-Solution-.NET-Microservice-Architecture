using MediatR;
using Notification.Application.Common;
using Notification.Application.Interfaces;

namespace Notification.Application.Features.Notifications.Commands;

public sealed class SendSmsNotificationCommandHandler
	: IRequestHandler<SendSmsNotificationCommand, CommandResponse>
{
	private readonly INotificationRepository _repository;
	private readonly IEmailTemplateService _templates;
	private readonly ISmsService _smsService;

	public SendSmsNotificationCommandHandler(
		INotificationRepository repository,
		IEmailTemplateService templates,
		ISmsService smsService)
	{
		_repository = repository;
		_templates = templates;
		_smsService = smsService;
	}

	public Task<CommandResponse> Handle(SendSmsNotificationCommand request, CancellationToken cancellationToken)
	{
		return SendNotificationHandler.HandleAsync(
			_repository,
			_templates,
			request.EventId,
			NotificationChannels.Sms,
			request.Recipient,
			request.TemplateName,
			request.Tokens,
			content => (null, null, content.Body),
			(content, token) => _smsService.SendAsync(request.Recipient, content.Body, token),
			cancellationToken);
	}
}
