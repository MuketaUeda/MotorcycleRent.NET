// ApplicationDbContext - Entity Framework DbContext
// Configuration of entities and relationships in the database

using Microsoft.EntityFrameworkCore;
using Moto.Domain.Entities;

namespace Moto.Infrastructure.DbContext;

/// <summary>
/// DbContext main application for motorcycle rental
/// </summary>
public class ApplicationDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    /// <summary>
    /// Constructor that receives the configuration options for the DbContext
    /// </summary>
    /// <param name="options">Configuration options for the DbContext</param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// DbSet for the Motorcycle entity
    /// </summary>
    public DbSet<Motorcycle> Motorcycles { get; set; }

    /// <summary>
    /// DbSet for the Courier entity
    /// </summary>
    public DbSet<Courier> Couriers { get; set; }

    /// <summary>
    /// DbSet for the Rental entity
    /// </summary>
    public DbSet<Rental> Rentals { get; set; }

    /// <summary>
    /// DbSet for the MotorcycleEvent entity
    /// </summary>
    public DbSet<MotorcycleEvent> MotorcycleEvents { get; set; }

    /// <summary>
    /// Configure entity relationships and constraints
    /// </summary>
    /// <param name="modelBuilder">Model builder for configuration</param>
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

        // Configure Courier entity with DateTime conversion
        modelBuilder.Entity<Courier>(entity =>
        {
            entity.Property(e => e.BirthDate)
                  .HasConversion(
                      v => DateTime.SpecifyKind(v, DateTimeKind.Utc),
                      v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
        });

        // Configure MotorcycleEvent entity
        modelBuilder.Entity<MotorcycleEvent>(entity =>
        {
            entity.Property(e => e.EventDate)
                  .HasConversion(
                      v => DateTime.SpecifyKind(v, DateTimeKind.Utc),
                      v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
            
            // Configure foreign key relationship
            entity.HasOne(e => e.Motorcycle)
                  .WithMany()
                  .HasForeignKey(e => e.MotorcycleId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
