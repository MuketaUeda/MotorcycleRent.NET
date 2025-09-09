// DependencyInjection - Configuration of dependency injection for the infrastructure layer
// Registers repositories, DbContext and other services from the infrastructure layer
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moto.Infrastructure.DbContext;
using Moto.Infrastructure.Repositories;
using Moto.Infrastructure.Services;
using Moto.Domain.Interfaces;
using Moto.Application.Interfaces;

namespace Moto.Infrastructure;

/// DependencyInjection - Configuration of dependency injection for the infrastructure layer
public static class DependencyInjection{
    /// Add infrastructure services to the container
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration){
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString, b => b.MigrationsAssembly("Moto.Infrastructure")));
        services.AddScoped<IMotorcycleRepository, MotorcycleRepository>();
        services.AddScoped<ICourierRepository, CourierRepository>();
        services.AddScoped<IRentalRepository, RentalRepository>();
        services.AddScoped<IMotorcycleEventRepository, MotorcycleEventRepository>();
        
        // Add Infrastructure Services (RabbitMQ Event Publisher)
        services.AddSingleton<IEventPublisher, RabbitMqEventPublisher>();
        
        return services;
    }
    
}