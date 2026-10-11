using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Notification.Infrastructure.Persistence;

public sealed class NotificationContextFactory : IDesignTimeDbContextFactory<NotificationContext>
{
	public NotificationContext CreateDbContext(string[] args)
	{
		var options = new DbContextOptionsBuilder<NotificationContext>()
			.UseSqlServer("Server=localhost,1436;Database=NotificationDb;Trusted_Connection=True;TrustServerCertificate=True")
			.Options;

		return new NotificationContext(options);
	}
}
