using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using User.API.Models;

namespace User.API.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<RoleUser> RoleUsers => Set<RoleUser>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<RoleUser>()
            .HasOne(ru => ru.ApplicationUser)
            .WithMany(u => u.RoleUsers)
            .HasForeignKey(ru => ru.UserId);

        builder.Entity<RoleUser>()
            .HasOne(ru => ru.ApplicationRole)
            .WithMany(r => r.RoleUsers)
            .HasForeignKey(ru => ru.RoleId);
    }
}

