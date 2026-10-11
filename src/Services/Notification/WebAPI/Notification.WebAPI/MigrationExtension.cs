using Microsoft.EntityFrameworkCore;
using Notification.Infrastructure.Persistence;

namespace Notification.WebAPI;

public static class MigrationExtension
{
	public static void ApplyMigration(this IApplicationBuilder app)
	{
		using IServiceScope scope = app.ApplicationServices.CreateScope();
		using NotificationContext context = scope.ServiceProvider.GetRequiredService<NotificationContext>();
		context.Database.Migrate();
	}
}
