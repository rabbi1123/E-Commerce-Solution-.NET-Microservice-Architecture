using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShoppingCart.Application.Persistence.Contracts;
using ShoppingCart.Infrastructure.Repositories;

namespace ShoppingCart.Infrastructure
{
	public static class ShoppingCartInfrastructureServicesRegistration
	{
		public static IServiceCollection ConfigureShoppingCartInfrastructureServices(this IServiceCollection services)
		{
			var defaultConnection = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");
			services.AddEntityFrameworkNpgsql()
				.AddDbContext<ShoppingCartDBContext>(opt =>
				opt.UseNpgsql(defaultConnection));

			services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
			services.AddScoped<ICartHeaderRepository, CartHeaderRepository>();

			return services;
		}
	}
}
