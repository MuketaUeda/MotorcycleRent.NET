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
        services.AddScoped<IMotorcycleService>(provider =>
        {
            var motorcycleRepository = provider.GetRequiredService<Moto.Domain.Interfaces.IMotorcycleRepository>();
            var rentalRepository = provider.GetRequiredService<Moto.Domain.Interfaces.IRentalRepository>();
            var mapper = provider.GetRequiredService<AutoMapper.IMapper>();
            var eventPublisher = provider.GetService<IEventPublisher>();
            
            return new MotorcycleService(motorcycleRepository, rentalRepository, mapper, eventPublisher);
        });
        
        services.AddScoped<ICourierService, CourierService>();
        services.AddScoped<IRentalService, RentalService>();
        
        // Add AutoMapper
        services.AddAutoMapper(typeof(DependencyInjection).Assembly);
        
        return services;
    }
}
