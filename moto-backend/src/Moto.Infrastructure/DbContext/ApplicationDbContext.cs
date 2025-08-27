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
}
