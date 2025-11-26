using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShoppingCart.Application.Persistence.Contracts;
using ShoppingCart.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Infrastructure
{
    public static class ShoppingCartInfrastructureServicesRegistration
    {
        public static IServiceCollection ConfigureShoppingCartInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEntityFrameworkNpgsql()
                .AddDbContext<ShoppingCartDBContext>(opt =>
                opt.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<ICartHeaderRepository, CartHeaderRepository>();
            services.AddStackExchangeRedisCache(options => options.Configuration = configuration.GetConnectionString("Cache"));

            return services;
        }
    }
}
