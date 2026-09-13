using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domain;

public class DatabaseContext : DbContext
{
    public DatabaseContext()
    {
    }

    // Allow passing DbContextOptions from tests or DI
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }

    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Resource> Resources { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured)
            return;

        const Environment.SpecialFolder folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        var dbPath = Path.Join(path, "ResourceBookingSystem.db");

        optionsBuilder.UseSqlite($"Data Source={dbPath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Booking>()
            .HasOne(b => b.Resource)
            .WithMany(r => r.Bookings)
            .HasForeignKey(b => b.ResourceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}