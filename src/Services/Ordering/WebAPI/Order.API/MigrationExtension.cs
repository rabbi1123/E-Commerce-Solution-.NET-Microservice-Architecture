using Microsoft.EntityFrameworkCore;
using Order.Infrastructure.Persistence;

namespace Order.API
{
    public static class MigrationExtension
    {
        public static void ApplyMigration(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();

            using OrderContext dBContext = scope.ServiceProvider.GetRequiredService<OrderContext>();
            dBContext.Database.Migrate();
        }
    }
}
