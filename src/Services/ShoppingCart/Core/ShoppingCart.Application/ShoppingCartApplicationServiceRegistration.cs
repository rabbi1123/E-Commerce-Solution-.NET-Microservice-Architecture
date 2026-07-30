using FluentValidation;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ShoppingCart.Application
{
	public static class ShoppingCartApplicationServiceRegistration
	{
		public static IServiceCollection ConfigureShoppingCartApplicationServices(this IServiceCollection services)
		{
			services.AddAutoMapper(Assembly.GetExecutingAssembly());
			services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

			services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

			services.AddMassTransit(config =>
			{
				config.SetKebabCaseEndpointNameFormatter();
				config.UsingRabbitMq((ctx, cfg) =>
				{
					cfg.Host(Environment.GetEnvironmentVariable("EventBusSettings__HostAddress"));
					cfg.ConfigureEndpoints(ctx);
				});
			});

			return services;
		}
	}
}
