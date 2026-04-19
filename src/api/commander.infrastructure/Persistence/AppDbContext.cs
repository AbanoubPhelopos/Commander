using commander.domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace commander.infrastructure.Persistence;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Platform> Platforms { get; set; }
    public DbSet<Command> Commands { get; set; }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        IEnumerable<EntityEntry> entries = ChangeTracker.Entries()
            .Where(e => e.Entity is ICreatedAtTrackable && e.State == EntityState.Added);

        foreach (EntityEntry entry in entries)
        {
            ((ICreatedAtTrackable)entry.Entity).CreatedAt = DateTime.UtcNow;
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        modelBuilder.Entity<Platform>()
            .HasMany(p => p.Commands)
            .WithOne(c => c.Platform)
            .HasForeignKey(c => c.PlatformId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Command>()
            .HasIndex(c => c.PlatformId)
            .HasDatabaseName("Index_Command_PlatformId");
    }
}
