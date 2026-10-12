using MediatR;
using Notification.Application.Interfaces;
using Notification.Domain.Entities;

namespace Notification.Application.Features.Notifications.Queries;

public sealed class GetNotificationsQueryHandler : IRequestHandler<GetNotificationsQuery, List<NotificationDto>>
{
	private readonly INotificationRepository _repository;

	public GetNotificationsQueryHandler(INotificationRepository repository)
	{
		_repository = repository;
	}

	public async Task<List<NotificationDto>> Handle(GetNotificationsQuery request, CancellationToken cancellationToken)
	{
		var messages = await _repository.GetAllAsync(cancellationToken);
		return messages.Select(Map).ToList();
	}

	internal static NotificationDto Map(NotificationMessage message)
	{
		return new NotificationDto
		{
			Id = message.Id,
			EventId = message.EventId,
			Channel = message.Channel,
			Recipient = message.Recipient,
			TemplateName = message.TemplateName,
			TemplateVersion = message.TemplateVersion,
			Title = message.Title,
			Subject = message.Subject,
			Body = message.Body,
			Status = message.Status,
			RetryCount = message.RetryCount,
			Provider = message.Provider,
			ProviderMessageId = message.ProviderMessageId,
			ErrorCode = message.ErrorCode,
			ErrorMessage = message.ErrorMessage,
			SentOn = message.SentOn,
			CreatedOn = message.CreatedOn,
			CreatedBy = message.CreatedBy
		};
	}
}
