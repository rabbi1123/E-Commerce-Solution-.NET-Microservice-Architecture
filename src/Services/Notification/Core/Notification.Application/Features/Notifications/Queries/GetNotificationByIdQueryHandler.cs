using MediatR;
using Notification.Application.Interfaces;

namespace Notification.Application.Features.Notifications.Queries;

public sealed class GetNotificationByIdQueryHandler : IRequestHandler<GetNotificationByIdQuery, NotificationDto?>
{
	private readonly INotificationRepository _repository;

	public GetNotificationByIdQueryHandler(INotificationRepository repository)
	{
		_repository = repository;
	}

	public async Task<NotificationDto?> Handle(GetNotificationByIdQuery request, CancellationToken cancellationToken)
	{
		var message = await _repository.GetByIdAsync(request.Id, cancellationToken);
		return message is null ? null : GetNotificationsQueryHandler.Map(message);
	}
}
