using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Notification.Application;

public static class NotificationApplicationServiceRegistration
{
	public static IServiceCollection AddNotificationApplication(this IServiceCollection services)
	{
		services.AddMediatR(Assembly.GetExecutingAssembly());
		return services;
	}
}
