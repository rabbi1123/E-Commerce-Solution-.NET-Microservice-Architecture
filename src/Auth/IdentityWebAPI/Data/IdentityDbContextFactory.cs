using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace IdentityWebAPI.Data;

public class IdentityDbContextFactory : IDesignTimeDbContextFactory<IdentityDbContext>
{
    public IdentityDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<IdentityDbContext>();
        optionsBuilder.UseSqlServer(
            "Server=localhost,1435;Database=IdentityDb;User Id=sa;Password=1234;TrustServerCertificate=True");

        return new IdentityDbContext(optionsBuilder.Options);
    }
}
