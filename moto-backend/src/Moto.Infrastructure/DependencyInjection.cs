// DependencyInjection - Configuração da injeção de dependência da infraestrutura
// Registra repositórios, DbContext e outros serviços da camada de infraestrutura
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moto.Infrastructure.DbContext;
using Moto.Infrastructure.Repositories;
using Moto.Domain.Interfaces;

namespace Moto.Infrastructure;

public static class DependencyInjection{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration){
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));
        services.AddScoped<IMotorcycleRepository, MotorcycleRepository>();
        return services;
    }
    
}