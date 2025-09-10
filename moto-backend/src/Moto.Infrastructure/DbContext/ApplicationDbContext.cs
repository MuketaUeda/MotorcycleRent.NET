// ApplicationDbContext - Entity Framework DbContext for motorcycle rental system
// Configuration of entities and relationships in the database

using Microsoft.EntityFrameworkCore;
using Moto.Domain.Entities;

namespace Moto.Infrastructure.DbContext;

/// DbContext main application for motorcycle rental
public class ApplicationDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    /// Constructor that receives the configuration options for the DbContext
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    /// DbSet for the Motorcycle entity
    public DbSet<Motorcycle> Motorcycles { get; set; }

    /// DbSet for the Courier entity
    public DbSet<Courier> Couriers { get; set; }

    /// DbSet for the Rental entity
    public DbSet<Rental> Rentals { get; set; }

    /// DbSet for the MotorcycleEvent entity
    public DbSet<MotorcycleEvent> MotorcycleEvents { get; set; }

    /// Configure entity relationships and constraints
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Motorcycle entity
        modelBuilder.Entity<Motorcycle>(entity =>
        {
            entity.HasIndex(e => e.Plate)
                  .IsUnique()
                  .HasDatabaseName("IX_Motorcycles_Plate_Unique");
        });

        // Configure Courier entity
        modelBuilder.Entity<Courier>(entity =>
        {
            entity.HasIndex(e => e.Cnpj)
                  .IsUnique()
                  .HasDatabaseName("IX_Couriers_Cnpj_Unique");
            
            entity.HasIndex(e => e.CnhNumber)
                  .IsUnique()
                  .HasDatabaseName("IX_Couriers_CnhNumber_Unique");
            
            entity.Property(e => e.BirthDate)
                  .HasConversion(
                      v => DateTime.SpecifyKind(v, DateTimeKind.Utc),
                      v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
        });

        // Configure Rental entity with DateTime conversions
        modelBuilder.Entity<Rental>(entity =>
        {
            entity.Property(e => e.StartDate)
                  .HasConversion(
                      v => DateTime.SpecifyKind(v, DateTimeKind.Utc),
                      v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

            entity.Property(e => e.ExpectedEndDate)
                  .HasConversion(
                      v => DateTime.SpecifyKind(v, DateTimeKind.Utc),
                      v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

            entity.Property(e => e.EndDate)
                  .HasConversion(
                      v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v,
                      v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v);
        });

        // Configure MotorcycleEvent entity
        modelBuilder.Entity<MotorcycleEvent>(entity =>
        {
            entity.Property(e => e.EventDate)
                  .HasConversion(
                      v => DateTime.SpecifyKind(v, DateTimeKind.Utc),
                      v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
        });
    }
}
