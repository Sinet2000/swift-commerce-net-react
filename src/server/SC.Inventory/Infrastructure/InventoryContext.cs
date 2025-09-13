using Microsoft.EntityFrameworkCore;
using SC.Inventory.Models;
using SC.Inventory.Utilities;

namespace SC.Inventory.Infrastructure;

public class InventoryContext : DbContext
{
    public InventoryContext(DbContextOptions<InventoryContext> options, IConfiguration configuration)
    : base(options)
    {
    }

    public DbSet<InventoryItem> InventoryItems { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<InventoryItem>(entity =>
        {
            entity.ToTable("Inventory");
            entity.Property(ci => ci.Name)
            .HasMaxLength(50);
            entity.HasIndex(ci => ci.Name);
        });

        // Add the outbox table to this context
        builder.UseIntegrationEventLogs();
    }
}
