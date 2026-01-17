//using AutoMapper.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Order.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Order.Application.Contracts.Persistence;
using Order.Infrastructure.Repositories;

namespace Order.Infrastructure
{
    public static class OrderInfrastructureServiceRegistration
    {
        public static IServiceCollection AddOrderInfrastructureServices(this IServiceCollection services, IConfiguration congiguration)
        {
            services.AddDbContext<OrderContext>(options => options.UseSqlServer(congiguration.GetConnectionString("OrderConnectionString")));
            services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));
            services.AddScoped<IOrderRepository, OrderRepository>();

            return services;
        }
    }
}
