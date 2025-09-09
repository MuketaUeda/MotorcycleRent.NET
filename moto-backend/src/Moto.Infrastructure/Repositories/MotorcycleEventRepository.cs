// MotorcycleEventRepository - Implement repository for motorcycle events
// Implement IMotorcycleEventRepository using Entity Framework
using Microsoft.EntityFrameworkCore;
using Moto.Domain.Entities;
using Moto.Domain.Interfaces;
using Moto.Infrastructure.DbContext;

namespace Moto.Infrastructure.Repositories;

public class MotorcycleEventRepository : IMotorcycleEventRepository
{
    private readonly ApplicationDbContext _context;

    public MotorcycleEventRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// Add a motorcycle event to the database
    public async Task<MotorcycleEvent> AddAsync(MotorcycleEvent motorcycleEvent)
    {
        await _context.MotorcycleEvents.AddAsync(motorcycleEvent);
        await _context.SaveChangesAsync();
        return motorcycleEvent;
    }
}
