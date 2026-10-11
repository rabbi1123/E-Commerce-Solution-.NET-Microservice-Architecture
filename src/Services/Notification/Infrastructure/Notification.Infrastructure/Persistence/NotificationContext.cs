using Microsoft.EntityFrameworkCore;
using Notification.Domain.Entities;

namespace Notification.Infrastructure.Persistence;

public class NotificationContext : DbContext
{
	public NotificationContext(DbContextOptions<NotificationContext> options) : base(options)
	{
	}

	public DbSet<NotificationMessage> Notifications { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<NotificationMessage>(entity =>
		{
			entity.ToTable("Notifications");
			entity.HasIndex(message => new { message.EventId, message.Channel }).IsUnique();
			entity.Property(message => message.Channel).HasMaxLength(20).IsRequired();
			entity.Property(message => message.Recipient).HasMaxLength(320).IsRequired();
			entity.Property(message => message.TemplateName).HasMaxLength(100);
			entity.Property(message => message.Title).HasMaxLength(200);
			entity.Property(message => message.Subject).HasMaxLength(200);
			entity.Property(message => message.Status).HasMaxLength(20).IsRequired();
			entity.Property(message => message.Provider).HasMaxLength(50);
			entity.Property(message => message.ProviderMessageId).HasMaxLength(100);
			entity.Property(message => message.ErrorCode).HasMaxLength(50);
			entity.Property(message => message.CreatedBy).HasMaxLength(100);
		});
	}
}
