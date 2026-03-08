using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ShoppingCart.Infrastructure;

namespace ShoppingCart.WebAPI
{
    public static class MigrationExtension
    {
        public static void ApplyMigration(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();

            using ShoppingCartDBContext dBContext = scope.ServiceProvider.GetRequiredService<ShoppingCartDBContext>();
            dBContext.Database.Migrate();
        }
    }
}
