// CourierRepository - Implementação do repositório de entregadores
// Implementa ICourierRepository usando Entity Framework
using Microsoft.EntityFrameworkCore;
using Moto.Domain.Interfaces;
using Moto.Domain.Entities;
using Moto.Infrastructure.DbContext;

namespace Moto.Infrastructure.Repositories;

public class CourierRepository : ICourierRepository
{
    private readonly ApplicationDbContext _context;

    public CourierRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Courier> AddAsync(Courier courier)
    {
        await _context.Couriers.AddAsync(courier);
        await _context.SaveChangesAsync();
        return courier;
    }

    public async Task<Courier?> GetByIdAsync(Guid id)
    {
        return await _context.Couriers.FindAsync(id);
    }

    public async Task<Courier?> GetByCnpjAsync(string cnpj)
    {
        return await _context.Couriers.FirstOrDefaultAsync(c => c.Cnpj == cnpj);
    }

    public async Task<Courier?> GetByCnhNumberAsync(string cnhNumber)
    {
        return await _context.Couriers.FirstOrDefaultAsync(c => c.CnhNumber == cnhNumber);
    }

    public async Task<IReadOnlyList<Courier>> GetAllAsync()
    {
        return await _context.Couriers.ToListAsync();
    }

    public async Task<Courier> UpdateAsync(Courier courier)
    {
        _context.Couriers.Update(courier);
        await _context.SaveChangesAsync();
        return courier;
    }

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
