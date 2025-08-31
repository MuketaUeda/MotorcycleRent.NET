// DependencyInjection - Application layer dependency injection configuration
// Registers application services and validators
using Microsoft.Extensions.DependencyInjection;
using Moto.Application.Services;
using Moto.Application.Interfaces;
using Moto.Application.Validators;
using FluentValidation;

namespace Moto.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register Application Services
        services.AddScoped<IMotorcycleService, MotorcycleService>();
        services.AddScoped<ICourierService, CourierService>();
        services.AddScoped<IRentalService, RentalService>();
        
        // Register FluentValidation Validators
        services.AddValidatorsFromAssemblyContaining<CreateMotorcycleDtoValidator>();
        
        return services;
    }
}
