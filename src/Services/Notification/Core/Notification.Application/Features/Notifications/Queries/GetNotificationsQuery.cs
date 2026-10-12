using MediatR;

namespace Notification.Application.Features.Notifications.Queries;

public sealed class GetNotificationsQuery : IRequest<List<NotificationDto>>
{
}
