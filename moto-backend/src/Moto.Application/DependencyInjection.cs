// DependencyInjection - Configuração da injeção de dependência da aplicação
// Registra serviços da camada de aplicação
using Microsoft.Extensions.DependencyInjection;
using Moto.Application.Services;

namespace Moto.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<MotorcycleService>();
        services.AddScoped<CourierService>();
        
        return services;
    }
}
