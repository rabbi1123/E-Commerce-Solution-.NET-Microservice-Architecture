using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notification.Application.Interfaces;
using Notification.Infrastructure.Email;
using Notification.Infrastructure.Persistence;
using Notification.Infrastructure.Push;
using Notification.Infrastructure.Sms;

namespace Notification.Infrastructure;

public static class NotificationInfrastructureServiceRegistration
{
	public static IServiceCollection AddNotificationInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{
		services
			.AddOptions<SmtpSettings>()
			.Bind(configuration.GetSection(SmtpSettings.SectionName))
			.ValidateDataAnnotations()
			.ValidateOnStart();

		services
			.AddOptions<PublicWebSettings>()
			.Bind(configuration.GetSection(PublicWebSettings.SectionName));

		var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__NotificationConnectionString")
			?? configuration.GetConnectionString("NotificationConnectionString");

		services.AddDbContext<NotificationContext>(options =>
			options.UseSqlServer(connectionString, sqlOptions => sqlOptions.EnableRetryOnFailure()));

		services.AddScoped<INotificationRepository, NotificationRepository>();
		services.AddSingleton<IEmailTemplateService, EmailTemplateService>();
		services.AddScoped<IEmailService, SmtpEmailService>();
		services.AddScoped<ISmsService, SmsService>();
		services.AddScoped<IPushService, PushService>();

		return services;
	}
}
