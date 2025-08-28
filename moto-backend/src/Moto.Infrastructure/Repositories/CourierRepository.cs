// CourierRepository - Implementação do repositório de entregadores
// Implementa ICourierRepository usando Entity Framework
using Microsoft.EntityFrameworkCore;
using Moto.Domain.Interfaces;
using Moto.Domain.Entities;
using Moto.Infrastructure.DbContext;

namespace Moto.Infrastructure.Repositories;

/// <summary>
/// CourierRepository - Implementação do repositório de entregadores
/// Implementa ICourierRepository usando Entity Framework
/// </summary>
public class CourierRepository : ICourierRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// CourierRepository - Implementação do repositório de entregadores
    /// Implementa ICourierRepository usando Entity Framework
    /// </summary>
    /// <param name="context">Contexto do banco de dados</param>
    public CourierRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Adiciona um novo entregador
    /// </summary>
    /// <param name="courier">Entregador a ser adicionado</param>
    /// <returns>Entregador adicionado</returns>
    public async Task<Courier> AddAsync(Courier courier)
    {
        await _context.Couriers.AddAsync(courier);
        await _context.SaveChangesAsync();
        return courier;
    }

    /// <summary>
    /// Obtém um entregador pelo ID
    /// </summary>
    /// <param name="id">ID do entregador</param>
    /// <returns>Entregador encontrado ou null se não encontrado</returns>
    public async Task<Courier?> GetByIdAsync(Guid id)
    {
        return await _context.Couriers.FindAsync(id);
    }

    /// <summary>
    /// Obtém um entregador pelo CNPJ
    /// </summary>
    /// <param name="cnpj">CNPJ do entregador</param>
    /// <returns>Entregador encontrado ou null se não encontrado</returns>
    public async Task<Courier?> GetByCnpjAsync(string cnpj)
    {
        return await _context.Couriers.FirstOrDefaultAsync(c => c.Cnpj == cnpj);
    }

    /// <summary>
    /// Obtém um entregador pelo número da CNH
    /// </summary>
    /// <param name="cnhNumber">Número da CNH do entregador</param>
    /// <returns>Entregador encontrado ou null se não encontrado</returns>
    public async Task<Courier?> GetByCnhNumberAsync(string cnhNumber)
    {
        return await _context.Couriers.FirstOrDefaultAsync(c => c.CnhNumber == cnhNumber);
    }

    /// <summary>
    /// Obtém todos os entregadores
    /// </summary>
    /// <returns>Lista de entregadores</returns>
    public async Task<IReadOnlyList<Courier>> GetAllAsync()
    {
        return await _context.Couriers.ToListAsync();
    }

    /// <summary>
    /// Atualiza um entregador
    /// </summary>
    /// <param name="courier">Entregador a ser atualizado</param>
    /// <returns>Entregador atualizado</returns>
    public async Task<Courier> UpdateAsync(Courier courier)
    {
        _context.Couriers.Update(courier);
        await _context.SaveChangesAsync();
        return courier;
    }

    /// <summary>
    /// Remove um entregador
    /// </summary>
    /// <param name="id">ID do entregador a ser removido</param>
    /// <returns>True se o entregador foi removido, false caso contrário</returns>
    public async Task<bool> DeleteAsync(Guid id)
    {
        var courier = await GetByIdAsync(id);
        if (courier != null)
        {
            _context.Couriers.Remove(courier);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }
}
