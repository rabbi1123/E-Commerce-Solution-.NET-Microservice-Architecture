using Microsoft.EntityFrameworkCore;
using Notification.Application.Interfaces;
using Notification.Domain.Entities;

namespace Notification.Infrastructure.Persistence;

public sealed class NotificationRepository : INotificationRepository
{
	private readonly NotificationContext _context;

	public NotificationRepository(NotificationContext context)
	{
		_context = context;
	}

	public async Task<long> AddAsync(NotificationMessage entity, CancellationToken cancellationToken = default)
	{
		_context.Notifications.Add(entity);
		await _context.SaveChangesAsync(cancellationToken);
		return entity.Id;
	}

	public Task<NotificationMessage?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
	{
		return _context.Notifications.AsNoTracking().FirstOrDefaultAsync(message => message.Id == id, cancellationToken);
	}

	public Task<List<NotificationMessage>> GetAllAsync(CancellationToken cancellationToken = default)
	{
		return _context.Notifications
			.AsNoTracking()
			.OrderByDescending(message => message.CreatedOn)
			.ToListAsync(cancellationToken);
	}

	public Task<bool> ExistsAsync(Guid eventId, string channel, CancellationToken cancellationToken = default)
	{
		return _context.Notifications.AnyAsync(
			message => message.EventId == eventId && message.Channel == channel,
			cancellationToken);
	}
}
