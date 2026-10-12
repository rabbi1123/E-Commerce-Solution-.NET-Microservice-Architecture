using Notification.Domain.Entities;

namespace Notification.Application.Interfaces;

public interface INotificationRepository
{
	Task<long> AddAsync(NotificationMessage entity, CancellationToken cancellationToken = default);

	Task<NotificationMessage?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

	Task<List<NotificationMessage>> GetAllAsync(CancellationToken cancellationToken = default);

	Task<bool> ExistsAsync(Guid eventId, string channel, CancellationToken cancellationToken = default);
}
