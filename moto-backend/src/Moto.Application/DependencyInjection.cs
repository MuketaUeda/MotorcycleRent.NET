// DependencyInjection - Configuração da injeção de dependência da aplicação
// Registra serviços da camada de aplicação
using Microsoft.Extensions.DependencyInjection;
using Moto.Application.Services;
using Moto.Application.Interfaces;

namespace Moto.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IMotorcycleService, MotorcycleService>();
        services.AddScoped<ICourierService, CourierService>();
        services.AddScoped<IRentalService, RentalService>();
        
        // Add AutoMapper
        services.AddAutoMapper(typeof(DependencyInjection).Assembly);
        
        return services;
    }
}
