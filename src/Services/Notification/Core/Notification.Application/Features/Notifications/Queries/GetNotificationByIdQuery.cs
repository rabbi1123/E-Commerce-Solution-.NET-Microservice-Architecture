using MediatR;

namespace Notification.Application.Features.Notifications.Queries;

public sealed class GetNotificationByIdQuery : IRequest<NotificationDto?>
{
	public long Id { get; init; }
}
