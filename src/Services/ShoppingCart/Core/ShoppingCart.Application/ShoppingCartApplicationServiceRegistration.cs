using EventBus.RabbitMQ;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ShoppingCart.Application
{
	public static class ShoppingCartApplicationServiceRegistration
	{
		public static IServiceCollection ConfigureShoppingCartApplicationServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddAutoMapper(Assembly.GetExecutingAssembly());
			services.AddMediatR(mediatr => mediatr.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

			services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

			services.AddRabbitMqEventBus(configuration);

			return services;
		}
	}
}
