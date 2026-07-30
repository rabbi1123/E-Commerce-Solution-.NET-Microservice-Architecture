using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Order.Application.Contracts.Persistence;
using Order.Infrastructure.Persistence;
using Order.Infrastructure.Repositories;

namespace Order.Infrastructure
{
	public static class OrderInfrastructureServiceRegistration
	{
		public static IServiceCollection AddOrderInfrastructureServices(this IServiceCollection services)
		{
			var orderConnectionString = Environment.GetEnvironmentVariable("ConnectionStrings__OrderConnectionString");
			services.AddDbContext<OrderContext>(options => options.UseSqlServer(orderConnectionString, sqlOptions =>
							sqlOptions.EnableRetryOnFailure()));
			services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));
			services.AddScoped<IOrderRepository, OrderRepository>();

			return services;
		}
	}
}
