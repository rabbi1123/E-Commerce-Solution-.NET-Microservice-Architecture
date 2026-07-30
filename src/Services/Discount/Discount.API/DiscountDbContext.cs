using Discount.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Discount.API
{
    public class DiscountDbContext : DbContext
    {
        public DbSet<ProductDiscount> Discount { get; set; }
        public DiscountDbContext(DbContextOptions<DiscountDbContext> dbContextOptions) : base(dbContextOptions)
        {
            try
            {
                var databaseCreator = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
                if (databaseCreator != null)
                {
                    if(!databaseCreator.CanConnect()) databaseCreator.Create();

                    if(!databaseCreator.HasTables()) databaseCreator.CreateTables();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
