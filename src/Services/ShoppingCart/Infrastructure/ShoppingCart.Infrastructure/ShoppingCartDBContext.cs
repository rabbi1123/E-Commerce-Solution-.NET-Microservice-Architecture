using Microsoft.EntityFrameworkCore;
using ShoppingCart.Domain.Common;
using ShoppingCart.Domain.Entity;

namespace ShoppingCart.Infrastructure
{
	public class ShoppingCartDBContext : DbContext
	{
		public ShoppingCartDBContext(DbContextOptions<ShoppingCartDBContext> options) : base(options)
		{

		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(ShoppingCartDBContext).Assembly);
		}

		public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			foreach (var entry in ChangeTracker.Entries<BaseDomainEntity>())
			{
				entry.Entity.LastModifiedDate = DateTime.UtcNow;
				if (entry.State == EntityState.Added)
				{
					entry.Entity.DateCreated = DateTime.UtcNow;
					entry.Entity.Status = "active";
				}
			}
			return base.SaveChangesAsync(cancellationToken);
		}

		public DbSet<CartHeader> CartHeaders { get; set; }
		public DbSet<CartDetails> CartDetails { get; set; }
	}
}
