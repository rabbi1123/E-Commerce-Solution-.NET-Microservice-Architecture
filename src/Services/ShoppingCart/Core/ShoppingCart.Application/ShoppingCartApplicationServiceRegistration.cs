using FluentValidation;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Application
{
    public static class ShoppingCartApplicationServiceRegistration
    {
        public static IServiceCollection ConfigureShoppingCartApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

			services.AddMassTransit(config => {
				config.SetKebabCaseEndpointNameFormatter();
				config.UsingRabbitMq((ctx, cfg) =>
				{
					cfg.Host(configuration["EventBusSettings:HostAddress"]);
					cfg.ConfigureEndpoints(ctx);
				});
			});

			return services;
        }
    }
}
