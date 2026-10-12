using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventBus.RabbitMQ;

public static class EventBusServiceCollectionExtensions
{
    public const string HostAddressKey = "EventBusSettings:HostAddress";

    public static IServiceCollection AddRabbitMqEventBus(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<IBusRegistrationConfigurator>? configure = null)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        var hostAddress = configuration[HostAddressKey];
        if (string.IsNullOrWhiteSpace(hostAddress))
        {
            throw new InvalidOperationException(
                "EventBusSettings:HostAddress is not configured. Set EventBusSettings__HostAddress or EventBusSettings:HostAddress.");
        }

        services.AddMassTransit(bus =>
        {
            bus.SetKebabCaseEndpointNameFormatter();
            configure?.Invoke(bus);

            bus.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(hostAddress);
                cfg.ConfigureEndpoints(context);
            });
        });

        services.AddScoped<IEventBus, MassTransitEventBus>();

        return services;
    }
}
