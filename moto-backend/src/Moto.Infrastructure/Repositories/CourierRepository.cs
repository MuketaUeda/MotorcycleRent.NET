// CourierRepository - Implement repository for couriers
// Implement ICourierRepository using Entity Framework
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

    /// Add a new courier
    public async Task<Courier> AddAsync(Courier courier)
    {
        await _context.Couriers.AddAsync(courier);
        await _context.SaveChangesAsync();
        return courier;
    }

    /// Get a courier by ID
    public async Task<Courier?> GetByIdAsync(string id)
    {
        return await _context.Couriers.FindAsync(id);
    }

    /// Get a courier by CNPJ
    public async Task<Courier?> GetByCnpjAsync(string cnpj)
    {
        return await _context.Couriers.FirstOrDefaultAsync(c => c.Cnpj == cnpj);
    }

    /// Get a courier by CNH number
    public async Task<Courier?> GetByCnhNumberAsync(string cnhNumber)
    {
        return await _context.Couriers.FirstOrDefaultAsync(c => c.CnhNumber == cnhNumber);
    }

    /// Update a courier
    public async Task<Courier> UpdateAsync(Courier courier)
    {
        _context.Couriers.Update(courier);
        await _context.SaveChangesAsync();
        return courier;
    }
}
